package main

import (
	"database/sql"
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"strings"

	_ "github.com/denisenkom/go-mssqldb"
	"gopkg.in/ini.v1"
)

type Config struct {
	Server     string
	Port       int
	User       string
	Password   string
	AllowedDBs []string
}

type QueryRequest struct {
	Database string  `json:"database"`
	SoTo     string  `json:"SoTo"`
	SoThua   string  `json:"SoThua"`
	TyLe     float64 `json:"TyLe"`
	KvhcId   float64 `json:"KvhcId"`
}

type Response struct {
	Status  string      `json:"status"`
	Message string      `json:"message"`
	Data    interface{} `json:"data,omitempty"`
}

var (
	appConfig Config
	dbPool    = make(map[string]*sql.DB)
)

func loadConfig(filename string) error {
	cfg, err := ini.Load(filename)
	if err != nil {
		return fmt.Errorf("failed to load config file: %v", err)
	}

	appConfig.Server = cfg.Section("database").Key("server").String()
	appConfig.Port, _ = cfg.Section("database").Key("port").Int()
	appConfig.User = cfg.Section("database").Key("user").String()
	appConfig.Password = cfg.Section("database").Key("password").String()

	allowedDBs := cfg.Section("databases").Key("allowed").String()
	appConfig.AllowedDBs = strings.Split(allowedDBs, ",")

	return nil
}

func isAllowedDatabase(database string) bool {
	for _, db := range appConfig.AllowedDBs {
		if db == database {
			return true
		}
	}
	return false
}

func handleDynamicQuery(w http.ResponseWriter, r *http.Request) {
	var req QueryRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		http.Error(w, "Invalid request", http.StatusBadRequest)
		return
	}

	if !isAllowedDatabase(req.Database) {
		http.Error(w, "Database (KVHC_ID) not allowed", http.StatusForbidden)
		return
	}

	connStr := fmt.Sprintf("server=%s;user id=%s;password=%s;port=%d;database=%s;encrypt=disable",
		appConfig.Server, appConfig.User, appConfig.Password, appConfig.Port, req.Database)
	db, err := sql.Open("mssql", connStr)
	if err != nil {
		http.Error(w, "Database connection error", http.StatusInternalServerError)
		return
	}
	defer db.Close()

	query := `
		SELECT eminx, eminy, emaxx, emaxy
		FROM sde.f18 
		INNER JOIN sde.THUADAT ON f18.fid = sde.THUADAT.shape
		WHERE LOWER(sde.THUADAT.SOTO) = LOWER(?) AND 
			  LOWER(sde.THUADAT.SOTHUA) = LOWER(?) AND 
			  CAST(sde.THUADAT.TYLE AS float) = ? AND 
			  CAST(sde.THUADAT.KVHC_ID AS float) = ?`

	var eminx, eminy, emaxx, emaxy float64
	err = db.QueryRow(query, req.SoTo, req.SoThua, req.TyLe, req.KvhcId).Scan(&eminx, &eminy, &emaxx, &emaxy)
	if err != nil {
		log.Printf("Query error: %v", err)
		http.Error(w, "No data found", http.StatusNotFound)
		return
	}

	coords := struct {
		X float64 `json:"x"`
		Y float64 `json:"y"`
	}{
		X: (eminx + emaxx) / 2,
		Y: (eminy + emaxy) / 2,
	}

	response := Response{
		Status:  "success",
		Message: "Lấy tọa độ thửa đất thành công",
		Data:    coords,
	}
	log.Printf("Query result: %v", coords)
	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(response)
}

func handleHealth(w http.ResponseWriter, r *http.Request) {
	response := Response{
		Status:  "success",
		Message: "Dịch vụ đang hoạt động",
	}
	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(response)
}

func main() {
	if err := loadConfig("./config/data.ini"); err != nil {
		log.Fatalf("Error loading config: %v", err)
	}

	http.HandleFunc("/api/sde/toadothuadat", handleDynamicQuery)
	http.HandleFunc("/api/health", handleHealth)

	port := 8151
	log.Printf("Server is running on port %d...", port)
	log.Fatal(http.ListenAndServe(fmt.Sprintf(":%d", port), nil))
}
