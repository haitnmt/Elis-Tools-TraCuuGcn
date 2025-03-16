var Xe = (i) => {
  throw TypeError(i);
};
var qe = (i, f, p) => f.has(i) || Xe("Cannot " + p);
var Ze = (i, f, p) => (qe(i, f, "read from private field"), p ? p.call(i) : f.get(i)), Ye = (i, f, p) => f.has(i) ? Xe("Cannot add the same private member more than once") : f instanceof WeakSet ? f.add(i) : f.set(i, p), Qe = (i, f, p, T) => (qe(i, f, "write to private field"), T ? T.call(i, p) : f.set(i, p), p);
const Bt = [
  ["Aztec", "M"],
  ["Codabar", "L"],
  ["Code39", "L"],
  ["Code93", "L"],
  ["Code128", "L"],
  ["DataBar", "L"],
  ["DataBarExpanded", "L"],
  ["DataMatrix", "M"],
  ["EAN-8", "L"],
  ["EAN-13", "L"],
  ["ITF", "L"],
  ["MaxiCode", "M"],
  ["PDF417", "M"],
  ["QRCode", "M"],
  ["UPC-A", "L"],
  ["UPC-E", "L"],
  ["MicroQRCode", "M"],
  ["rMQRCode", "M"],
  ["DXFilmEdge", "L"],
  ["DataBarLimited", "L"]
], Ut = Bt.map(([i]) => i), Za = Ut.filter(
  (i, f) => Bt[f][1] === "L"
), Ya = Ut.filter(
  (i, f) => Bt[f][1] === "M"
);
function re(i) {
  switch (i) {
    case "Linear-Codes":
      return Za.reduce((f, p) => f | re(p), 0);
    case "Matrix-Codes":
      return Ya.reduce((f, p) => f | re(p), 0);
    case "Any":
      return (1 << Bt.length) - 1;
    case "None":
      return 0;
    default:
      return 1 << Ut.indexOf(i);
  }
}
function Qa(i) {
  if (i === 0)
    return "None";
  const f = 31 - Math.clz32(i);
  return Ut[f];
}
function Ja(i) {
  return i.reduce((f, p) => f | re(p), 0);
}
const Ka = [
  "LocalAverage",
  "GlobalHistogram",
  "FixedThreshold",
  "BoolCast"
];
function ti(i) {
  return Ka.indexOf(i);
}
const Je = [
  "Unknown",
  "ASCII",
  "ISO8859_1",
  "ISO8859_2",
  "ISO8859_3",
  "ISO8859_4",
  "ISO8859_5",
  "ISO8859_6",
  "ISO8859_7",
  "ISO8859_8",
  "ISO8859_9",
  "ISO8859_10",
  "ISO8859_11",
  "ISO8859_13",
  "ISO8859_14",
  "ISO8859_15",
  "ISO8859_16",
  "Cp437",
  "Cp1250",
  "Cp1251",
  "Cp1252",
  "Cp1256",
  "Shift_JIS",
  "Big5",
  "GB2312",
  "GB18030",
  "EUC_JP",
  "EUC_KR",
  "UTF16BE",
  /**
   * UnicodeBig [[deprecated]]
   */
  "UTF16BE",
  "UTF8",
  "UTF16LE",
  "UTF32BE",
  "UTF32LE",
  "BINARY"
];
function ei(i) {
  return i === "UnicodeBig" ? Je.indexOf("UTF16BE") : Je.indexOf(i);
}
const ri = [
  "Text",
  "Binary",
  "Mixed",
  "GS1",
  "ISO15434",
  "UnknownECI"
];
function ni(i) {
  return ri[i];
}
const ai = ["Ignore", "Read", "Require"];
function ii(i) {
  return ai.indexOf(i);
}
const oi = ["Plain", "ECI", "HRI", "Hex", "Escaped"];
function si(i) {
  return oi.indexOf(i);
}
const Lt = {
  formats: [],
  tryHarder: !0,
  tryRotate: !0,
  tryInvert: !0,
  tryDownscale: !0,
  tryDenoise: !1,
  binarizer: "LocalAverage",
  isPure: !1,
  downscaleFactor: 3,
  downscaleThreshold: 500,
  minLineCount: 2,
  maxNumberOfSymbols: 255,
  tryCode39ExtendedMode: !0,
  returnErrors: !1,
  eanAddOnSymbol: "Ignore",
  textMode: "HRI",
  characterSet: "Unknown"
};
function Ke(i) {
  return {
    ...i,
    formats: Ja(i.formats),
    binarizer: ti(i.binarizer),
    eanAddOnSymbol: ii(i.eanAddOnSymbol),
    textMode: si(i.textMode),
    characterSet: ei(i.characterSet)
  };
}
function ui(i) {
  return {
    ...i,
    format: Qa(i.format),
    contentType: ni(i.contentType),
    eccLevel: i.ecLevel
  };
}
const Di = "2.1.0", Ii = "37b847798a1af55d3a289a9516a751fcafae3c23", ci = {
  locateFile: (i, f) => {
    const p = i.match(/_(.+?)\.wasm$/);
    return p ? `https://fastly.jsdelivr.net/npm/zxing-wasm@2.1.0/dist/${p[1]}/${i}` : f + i;
  }
}, Rt = /* @__PURE__ */ new WeakMap();
function li(i, f) {
  return Object.is(i, f) || Object.keys(i).length === Object.keys(f).length && Object.keys(i).every(
    (p) => Object.prototype.hasOwnProperty.call(f, p) && i[p] === f[p]
  );
}
function er(i, {
  overrides: f,
  equalityFn: p = li,
  fireImmediately: T = !1
} = {}) {
  var c;
  const [O, D] = (c = Rt.get(i)) != null ? c : [ci], R = f != null ? f : O;
  let A;
  if (T) {
    if (D && (A = p(O, R)))
      return D;
    const M = i({
      ...R
    });
    return Rt.set(i, [R, M]), M;
  }
  (A != null ? A : p(O, R)) || Rt.set(i, [R]);
}
function fi(i) {
  Rt.delete(i);
}
async function di(i, f, p = Lt) {
  const T = {
    ...Lt,
    ...p
  }, c = await er(i, {
    fireImmediately: !0
  });
  let O, D;
  if ("width" in f && "height" in f && "data" in f) {
    const {
      data: A,
      data: { byteLength: M },
      width: k,
      height: Z
    } = f;
    D = c._malloc(M), c.HEAPU8.set(A, D), O = c.readBarcodesFromPixmap(
      D,
      k,
      Z,
      Ke(T)
    );
  } else {
    let A, M;
    if ("buffer" in f)
      [A, M] = [f.byteLength, f];
    else if ("byteLength" in f)
      [A, M] = [f.byteLength, new Uint8Array(f)];
    else if ("size" in f)
      [A, M] = [f.size, new Uint8Array(await f.arrayBuffer())];
    else
      throw new TypeError("Invalid input type");
    D = c._malloc(A), c.HEAPU8.set(M, D), O = c.readBarcodesFromImage(
      D,
      A,
      Ke(T)
    );
  }
  c._free(D);
  const R = [];
  for (let A = 0; A < O.size(); ++A)
    R.push(
      ui(O.get(A))
    );
  return R;
}
({
  ...Lt,
  formats: [...Lt.formats]
});
var ae = (() => {
  var i, f = typeof document < "u" && ((i = document.currentScript) == null ? void 0 : i.tagName.toUpperCase()) === "SCRIPT" ? document.currentScript.src : void 0;
  return function(p = {}) {
    var T, c = p, O, D, R = new Promise((t, e) => {
      O = t, D = e;
    }), A = typeof window == "object", M = typeof Bun < "u", k = typeof WorkerGlobalScope < "u";
    typeof process == "object" && typeof process.versions == "object" && typeof process.versions.node == "string" && process.type != "renderer";
    var Z = Object.assign({}, c), tt = "./this.program", L = "";
    function yt(t) {
      return c.locateFile ? c.locateFile(t, L) : L + t;
    }
    var it, ot;
    if (A || k || M) {
      var st;
      k ? L = self.location.href : typeof document < "u" && ((st = document.currentScript) === null || st === void 0 ? void 0 : st.tagName.toUpperCase()) === "SCRIPT" && (L = document.currentScript.src), f && (L = f), L.startsWith("blob:") ? L = "" : L = L.substr(0, L.replace(/[?#].*/, "").lastIndexOf("/") + 1), k && (ot = (t) => {
        var e = new XMLHttpRequest();
        return e.open("GET", t, !1), e.responseType = "arraybuffer", e.send(null), new Uint8Array(e.response);
      }), it = async (t) => {
        var e = await fetch(t, {
          credentials: "same-origin"
        });
        if (e.ok)
          return e.arrayBuffer();
        throw new Error(e.status + " : " + e.url);
      };
    }
    var dr = c.print || console.log.bind(console), et = c.printErr || console.error.bind(console);
    Object.assign(c, Z), Z = null, c.arguments && c.arguments, c.thisProgram && (tt = c.thisProgram);
    var vt = c.wasmBinary, mt, ie = !1, H, B, ut, gt, rt, P, oe, se;
    function ue() {
      var t = mt.buffer;
      c.HEAP8 = H = new Int8Array(t), c.HEAP16 = ut = new Int16Array(t), c.HEAPU8 = B = new Uint8Array(t), c.HEAPU16 = gt = new Uint16Array(t), c.HEAP32 = rt = new Int32Array(t), c.HEAPU32 = P = new Uint32Array(t), c.HEAPF32 = oe = new Float32Array(t), c.HEAPF64 = se = new Float64Array(t);
    }
    var ce = [], le = [], fe = [];
    function hr() {
      if (c.preRun)
        for (typeof c.preRun == "function" && (c.preRun = [c.preRun]); c.preRun.length; )
          vr(c.preRun.shift());
      kt(ce);
    }
    function pr() {
      kt(le);
    }
    function yr() {
      if (c.postRun)
        for (typeof c.postRun == "function" && (c.postRun = [c.postRun]); c.postRun.length; )
          gr(c.postRun.shift());
      kt(fe);
    }
    function vr(t) {
      ce.unshift(t);
    }
    function mr(t) {
      le.unshift(t);
    }
    function gr(t) {
      fe.unshift(t);
    }
    var Y = 0, ct = null;
    function wr(t) {
      var e;
      Y++, (e = c.monitorRunDependencies) === null || e === void 0 || e.call(c, Y);
    }
    function $r(t) {
      var e;
      if (Y--, (e = c.monitorRunDependencies) === null || e === void 0 || e.call(c, Y), Y == 0 && ct) {
        var r = ct;
        ct = null, r();
      }
    }
    function Wt(t) {
      var e;
      (e = c.onAbort) === null || e === void 0 || e.call(c, t), t = "Aborted(" + t + ")", et(t), ie = !0, t += ". Build with -sASSERTIONS for more info.";
      var r = new WebAssembly.RuntimeError(t);
      throw D(r), r;
    }
    var br = "data:application/octet-stream;base64,", de = (t) => t.startsWith(br);
    function Cr() {
      var t = "zxing_reader.wasm";
      return de(t) ? t : yt(t);
    }
    var wt;
    function Tr(t) {
      if (t == wt && vt)
        return new Uint8Array(vt);
      if (ot)
        return ot(t);
      throw "both async and sync fetching of the wasm failed";
    }
    async function Pr(t) {
      if (!vt)
        try {
          var e = await it(t);
          return new Uint8Array(e);
        } catch {
        }
      return Tr(t);
    }
    async function _r(t, e) {
      try {
        var r = await Pr(t), n = await WebAssembly.instantiate(r, e);
        return n;
      } catch (a) {
        et(`failed to asynchronously prepare wasm: ${a}`), Wt(a);
      }
    }
    async function Er(t, e, r) {
      if (!t && typeof WebAssembly.instantiateStreaming == "function" && !de(e) && typeof fetch == "function")
        try {
          var n = fetch(e, {
            credentials: "same-origin"
          }), a = await WebAssembly.instantiateStreaming(n, r);
          return a;
        } catch (o) {
          et(`wasm streaming compile failed: ${o}`), et("falling back to ArrayBuffer instantiation");
        }
      return _r(e, r);
    }
    function Or() {
      return {
        a: ha
      };
    }
    async function Ar() {
      var t;
      function e(o, u) {
        return E = o.exports, mt = E.xa, ue(), Ce = E.Ba, mr(E.ya), $r(), E;
      }
      wr();
      function r(o) {
        e(o.instance);
      }
      var n = Or();
      if (c.instantiateWasm)
        try {
          return c.instantiateWasm(n, e);
        } catch (o) {
          et(`Module.instantiateWasm callback failed with error: ${o}`), D(o);
        }
      (t = wt) !== null && t !== void 0 || (wt = Cr());
      try {
        var a = await Er(vt, wt, n);
        return r(a), a;
      } catch (o) {
        D(o);
        return;
      }
    }
    var kt = (t) => {
      for (; t.length > 0; )
        t.shift()(c);
    };
    c.noExitRuntime;
    var m = (t) => je(t), g = () => Re(), $t = [], bt = 0, Sr = (t) => {
      var e = new Ht(t);
      return e.get_caught() || (e.set_caught(!0), bt--), e.set_rethrown(!1), $t.push(e), Be(t), Me(t);
    }, V = 0, xr = () => {
      v(0, 0);
      var t = $t.pop();
      Le(t.excPtr), V = 0;
    };
    class Ht {
      constructor(e) {
        this.excPtr = e, this.ptr = e - 24;
      }
      set_type(e) {
        P[this.ptr + 4 >> 2] = e;
      }
      get_type() {
        return P[this.ptr + 4 >> 2];
      }
      set_destructor(e) {
        P[this.ptr + 8 >> 2] = e;
      }
      get_destructor() {
        return P[this.ptr + 8 >> 2];
      }
      set_caught(e) {
        e = e ? 1 : 0, H[this.ptr + 12] = e;
      }
      get_caught() {
        return H[this.ptr + 12] != 0;
      }
      set_rethrown(e) {
        e = e ? 1 : 0, H[this.ptr + 13] = e;
      }
      get_rethrown() {
        return H[this.ptr + 13] != 0;
      }
      init(e, r) {
        this.set_adjusted_ptr(0), this.set_type(e), this.set_destructor(r);
      }
      set_adjusted_ptr(e) {
        P[this.ptr + 16 >> 2] = e;
      }
      get_adjusted_ptr() {
        return P[this.ptr + 16 >> 2];
      }
    }
    var Dr = (t) => {
      throw V || (V = t), V;
    }, Ct = (t) => Fe(t), Vt = (t) => {
      var e = V;
      if (!e)
        return Ct(0), 0;
      var r = new Ht(e);
      r.set_adjusted_ptr(e);
      var n = r.get_type();
      if (!n)
        return Ct(0), e;
      for (var a of t) {
        if (a === 0 || a === n)
          break;
        var o = r.ptr + 16;
        if (Ue(a, n, o))
          return Ct(a), e;
      }
      return Ct(n), e;
    }, Ir = () => Vt([]), Mr = (t) => Vt([t]), Fr = (t, e) => Vt([t, e]), jr = () => {
      var t = $t.pop();
      t || Wt("no exception to throw");
      var e = t.excPtr;
      throw t.get_rethrown() || ($t.push(t), t.set_rethrown(!0), t.set_caught(!1), bt++), V = e, V;
    }, Rr = (t, e, r) => {
      var n = new Ht(t);
      throw n.init(e, r), V = t, bt++, V;
    }, Lr = () => bt, Br = () => Wt(""), Tt = {}, Nt = (t) => {
      for (; t.length; ) {
        var e = t.pop(), r = t.pop();
        r(e);
      }
    };
    function lt(t) {
      return this.fromWireType(P[t >> 2]);
    }
    var nt = {}, Q = {}, Pt = {}, he, _t = (t) => {
      throw new he(t);
    }, J = (t, e, r) => {
      t.forEach((s) => Pt[s] = e);
      function n(s) {
        var l = r(s);
        l.length !== t.length && _t("Mismatched type converter count");
        for (var d = 0; d < t.length; ++d)
          W(t[d], l[d]);
      }
      var a = new Array(e.length), o = [], u = 0;
      e.forEach((s, l) => {
        Q.hasOwnProperty(s) ? a[l] = Q[s] : (o.push(s), nt.hasOwnProperty(s) || (nt[s] = []), nt[s].push(() => {
          a[l] = Q[s], ++u, u === o.length && n(a);
        }));
      }), o.length === 0 && n(a);
    }, Ur = (t) => {
      var e = Tt[t];
      delete Tt[t];
      var r = e.rawConstructor, n = e.rawDestructor, a = e.fields, o = a.map((u) => u.getterReturnType).concat(a.map((u) => u.setterArgumentType));
      J([t], o, (u) => {
        var s = {};
        return a.forEach((l, d) => {
          var h = l.fieldName, y = u[d], $ = l.getter, C = l.getterContext, _ = u[d + a.length], I = l.setter, S = l.setterContext;
          s[h] = {
            read: (x) => y.fromWireType($(C, x)),
            write: (x, K) => {
              var j = [];
              I(S, x, _.toWireType(j, K)), Nt(j);
            }
          };
        }), [{
          name: e.name,
          fromWireType: (l) => {
            var d = {};
            for (var h in s)
              d[h] = s[h].read(l);
            return n(l), d;
          },
          toWireType: (l, d) => {
            for (var h in s)
              if (!(h in d))
                throw new TypeError(`Missing field: "${h}"`);
            var y = r();
            for (h in s)
              s[h].write(y, d[h]);
            return l !== null && l.push(n, y), y;
          },
          argPackAdvance: N,
          readValueFromPointer: lt,
          destructorFunction: n
        }];
      });
    }, Wr = (t, e, r, n, a) => {
    }, kr = () => {
      for (var t = new Array(256), e = 0; e < 256; ++e)
        t[e] = String.fromCharCode(e);
      pe = t;
    }, pe, F = (t) => {
      for (var e = "", r = t; B[r]; )
        e += pe[B[r++]];
      return e;
    }, at, b = (t) => {
      throw new at(t);
    };
    function Hr(t, e) {
      let r = arguments.length > 2 && arguments[2] !== void 0 ? arguments[2] : {};
      var n = e.name;
      if (t || b(`type "${n}" must have a positive integer typeid pointer`), Q.hasOwnProperty(t)) {
        if (r.ignoreDuplicateRegistrations)
          return;
        b(`Cannot register type '${n}' twice`);
      }
      if (Q[t] = e, delete Pt[t], nt.hasOwnProperty(t)) {
        var a = nt[t];
        delete nt[t], a.forEach((o) => o());
      }
    }
    function W(t, e) {
      let r = arguments.length > 2 && arguments[2] !== void 0 ? arguments[2] : {};
      return Hr(t, e, r);
    }
    var N = 8, Vr = (t, e, r, n) => {
      e = F(e), W(t, {
        name: e,
        fromWireType: function(a) {
          return !!a;
        },
        toWireType: function(a, o) {
          return o ? r : n;
        },
        argPackAdvance: N,
        readValueFromPointer: function(a) {
          return this.fromWireType(B[a]);
        },
        destructorFunction: null
      });
    }, Nr = (t) => ({
      count: t.count,
      deleteScheduled: t.deleteScheduled,
      preservePointerOnDelete: t.preservePointerOnDelete,
      ptr: t.ptr,
      ptrType: t.ptrType,
      smartPtr: t.smartPtr,
      smartPtrType: t.smartPtrType
    }), zt = (t) => {
      function e(r) {
        return r.$$.ptrType.registeredClass.name;
      }
      b(e(t) + " instance already deleted");
    }, Gt = !1, ye = (t) => {
    }, zr = (t) => {
      t.smartPtr ? t.smartPtrType.rawDestructor(t.smartPtr) : t.ptrType.registeredClass.rawDestructor(t.ptr);
    }, ve = (t) => {
      t.count.value -= 1;
      var e = t.count.value === 0;
      e && zr(t);
    }, me = (t, e, r) => {
      if (e === r)
        return t;
      if (r.baseClass === void 0)
        return null;
      var n = me(t, e, r.baseClass);
      return n === null ? null : r.downcast(n);
    }, ge = {}, Gr = {}, Xr = (t, e) => {
      for (e === void 0 && b("ptr should not be undefined"); t.baseClass; )
        e = t.upcast(e), t = t.baseClass;
      return e;
    }, qr = (t, e) => (e = Xr(t, e), Gr[e]), Et = (t, e) => {
      (!e.ptrType || !e.ptr) && _t("makeClassHandle requires ptr and ptrType");
      var r = !!e.smartPtrType, n = !!e.smartPtr;
      return r !== n && _t("Both smartPtrType and smartPtr must be specified"), e.count = {
        value: 1
      }, ft(Object.create(t, {
        $$: {
          value: e,
          writable: !0
        }
      }));
    };
    function Zr(t) {
      var e = this.getPointee(t);
      if (!e)
        return this.destructor(t), null;
      var r = qr(this.registeredClass, e);
      if (r !== void 0) {
        if (r.$$.count.value === 0)
          return r.$$.ptr = e, r.$$.smartPtr = t, r.clone();
        var n = r.clone();
        return this.destructor(t), n;
      }
      function a() {
        return this.isSmartPointer ? Et(this.registeredClass.instancePrototype, {
          ptrType: this.pointeeType,
          ptr: e,
          smartPtrType: this,
          smartPtr: t
        }) : Et(this.registeredClass.instancePrototype, {
          ptrType: this,
          ptr: t
        });
      }
      var o = this.registeredClass.getActualType(e), u = ge[o];
      if (!u)
        return a.call(this);
      var s;
      this.isConst ? s = u.constPointerType : s = u.pointerType;
      var l = me(e, this.registeredClass, s.registeredClass);
      return l === null ? a.call(this) : this.isSmartPointer ? Et(s.registeredClass.instancePrototype, {
        ptrType: s,
        ptr: l,
        smartPtrType: this,
        smartPtr: t
      }) : Et(s.registeredClass.instancePrototype, {
        ptrType: s,
        ptr: l
      });
    }
    var ft = (t) => typeof FinalizationRegistry > "u" ? (ft = (e) => e, t) : (Gt = new FinalizationRegistry((e) => {
      ve(e.$$);
    }), ft = (e) => {
      var r = e.$$, n = !!r.smartPtr;
      if (n) {
        var a = {
          $$: r
        };
        Gt.register(e, a, e);
      }
      return e;
    }, ye = (e) => Gt.unregister(e), ft(t)), Yr = () => {
      Object.assign(Ot.prototype, {
        isAliasOf(t) {
          if (!(this instanceof Ot) || !(t instanceof Ot))
            return !1;
          var e = this.$$.ptrType.registeredClass, r = this.$$.ptr;
          t.$$ = t.$$;
          for (var n = t.$$.ptrType.registeredClass, a = t.$$.ptr; e.baseClass; )
            r = e.upcast(r), e = e.baseClass;
          for (; n.baseClass; )
            a = n.upcast(a), n = n.baseClass;
          return e === n && r === a;
        },
        clone() {
          if (this.$$.ptr || zt(this), this.$$.preservePointerOnDelete)
            return this.$$.count.value += 1, this;
          var t = ft(Object.create(Object.getPrototypeOf(this), {
            $$: {
              value: Nr(this.$$)
            }
          }));
          return t.$$.count.value += 1, t.$$.deleteScheduled = !1, t;
        },
        delete() {
          this.$$.ptr || zt(this), this.$$.deleteScheduled && !this.$$.preservePointerOnDelete && b("Object already scheduled for deletion"), ye(this), ve(this.$$), this.$$.preservePointerOnDelete || (this.$$.smartPtr = void 0, this.$$.ptr = void 0);
        },
        isDeleted() {
          return !this.$$.ptr;
        },
        deleteLater() {
          return this.$$.ptr || zt(this), this.$$.deleteScheduled && !this.$$.preservePointerOnDelete && b("Object already scheduled for deletion"), this.$$.deleteScheduled = !0, this;
        }
      });
    };
    function Ot() {
    }
    var At = (t, e) => Object.defineProperty(e, "name", {
      value: t
    }), we = (t, e, r) => {
      if (t[e].overloadTable === void 0) {
        var n = t[e];
        t[e] = function() {
          for (var a = arguments.length, o = new Array(a), u = 0; u < a; u++)
            o[u] = arguments[u];
          return t[e].overloadTable.hasOwnProperty(o.length) || b(`Function '${r}' called with an invalid number of arguments (${o.length}) - expects one of (${t[e].overloadTable})!`), t[e].overloadTable[o.length].apply(this, o);
        }, t[e].overloadTable = [], t[e].overloadTable[n.argCount] = n;
      }
    }, $e = (t, e, r) => {
      c.hasOwnProperty(t) ? ((r === void 0 || c[t].overloadTable !== void 0 && c[t].overloadTable[r] !== void 0) && b(`Cannot register public name '${t}' twice`), we(c, t, t), c[t].overloadTable.hasOwnProperty(r) && b(`Cannot register multiple overloads of a function with the same number of arguments (${r})!`), c[t].overloadTable[r] = e) : (c[t] = e, c[t].argCount = r);
    }, Qr = 48, Jr = 57, Kr = (t) => {
      t = t.replace(/[^a-zA-Z0-9_]/g, "$");
      var e = t.charCodeAt(0);
      return e >= Qr && e <= Jr ? `_${t}` : t;
    };
    function tn(t, e, r, n, a, o, u, s) {
      this.name = t, this.constructor = e, this.instancePrototype = r, this.rawDestructor = n, this.baseClass = a, this.getActualType = o, this.upcast = u, this.downcast = s, this.pureVirtualFunctions = [];
    }
    var Xt = (t, e, r) => {
      for (; e !== r; )
        e.upcast || b(`Expected null or instance of ${r.name}, got an instance of ${e.name}`), t = e.upcast(t), e = e.baseClass;
      return t;
    };
    function en(t, e) {
      if (e === null)
        return this.isReference && b(`null is not a valid ${this.name}`), 0;
      e.$$ || b(`Cannot pass "${Jt(e)}" as a ${this.name}`), e.$$.ptr || b(`Cannot pass deleted object as a pointer of type ${this.name}`);
      var r = e.$$.ptrType.registeredClass, n = Xt(e.$$.ptr, r, this.registeredClass);
      return n;
    }
    function rn(t, e) {
      var r;
      if (e === null)
        return this.isReference && b(`null is not a valid ${this.name}`), this.isSmartPointer ? (r = this.rawConstructor(), t !== null && t.push(this.rawDestructor, r), r) : 0;
      (!e || !e.$$) && b(`Cannot pass "${Jt(e)}" as a ${this.name}`), e.$$.ptr || b(`Cannot pass deleted object as a pointer of type ${this.name}`), !this.isConst && e.$$.ptrType.isConst && b(`Cannot convert argument of type ${e.$$.smartPtrType ? e.$$.smartPtrType.name : e.$$.ptrType.name} to parameter type ${this.name}`);
      var n = e.$$.ptrType.registeredClass;
      if (r = Xt(e.$$.ptr, n, this.registeredClass), this.isSmartPointer)
        switch (e.$$.smartPtr === void 0 && b("Passing raw pointer to smart pointer is illegal"), this.sharingPolicy) {
          case 0:
            e.$$.smartPtrType === this ? r = e.$$.smartPtr : b(`Cannot convert argument of type ${e.$$.smartPtrType ? e.$$.smartPtrType.name : e.$$.ptrType.name} to parameter type ${this.name}`);
            break;
          case 1:
            r = e.$$.smartPtr;
            break;
          case 2:
            if (e.$$.smartPtrType === this)
              r = e.$$.smartPtr;
            else {
              var a = e.clone();
              r = this.rawShare(r, G.toHandle(() => a.delete())), t !== null && t.push(this.rawDestructor, r);
            }
            break;
          default:
            b("Unsupporting sharing policy");
        }
      return r;
    }
    function nn(t, e) {
      if (e === null)
        return this.isReference && b(`null is not a valid ${this.name}`), 0;
      e.$$ || b(`Cannot pass "${Jt(e)}" as a ${this.name}`), e.$$.ptr || b(`Cannot pass deleted object as a pointer of type ${this.name}`), e.$$.ptrType.isConst && b(`Cannot convert argument of type ${e.$$.ptrType.name} to parameter type ${this.name}`);
      var r = e.$$.ptrType.registeredClass, n = Xt(e.$$.ptr, r, this.registeredClass);
      return n;
    }
    var an = () => {
      Object.assign(St.prototype, {
        getPointee(t) {
          return this.rawGetPointee && (t = this.rawGetPointee(t)), t;
        },
        destructor(t) {
          var e;
          (e = this.rawDestructor) === null || e === void 0 || e.call(this, t);
        },
        argPackAdvance: N,
        readValueFromPointer: lt,
        fromWireType: Zr
      });
    };
    function St(t, e, r, n, a, o, u, s, l, d, h) {
      this.name = t, this.registeredClass = e, this.isReference = r, this.isConst = n, this.isSmartPointer = a, this.pointeeType = o, this.sharingPolicy = u, this.rawGetPointee = s, this.rawConstructor = l, this.rawShare = d, this.rawDestructor = h, !a && e.baseClass === void 0 ? n ? (this.toWireType = en, this.destructorFunction = null) : (this.toWireType = nn, this.destructorFunction = null) : this.toWireType = rn;
    }
    var be = (t, e, r) => {
      c.hasOwnProperty(t) || _t("Replacing nonexistent public symbol"), c[t].overloadTable !== void 0 && r !== void 0 ? c[t].overloadTable[r] = e : (c[t] = e, c[t].argCount = r);
    }, on = (t, e, r) => {
      t = t.replace(/p/g, "i");
      var n = c["dynCall_" + t];
      return n(e, ...r);
    }, xt = [], Ce, w = (t) => {
      var e = xt[t];
      return e || (t >= xt.length && (xt.length = t + 1), xt[t] = e = Ce.get(t)), e;
    }, sn = function(t, e) {
      let r = arguments.length > 2 && arguments[2] !== void 0 ? arguments[2] : [];
      if (t.includes("j"))
        return on(t, e, r);
      var n = w(e)(...r);
      return n;
    }, un = (t, e) => function() {
      for (var r = arguments.length, n = new Array(r), a = 0; a < r; a++)
        n[a] = arguments[a];
      return sn(t, e, n);
    }, U = (t, e) => {
      t = F(t);
      function r() {
        return t.includes("j") ? un(t, e) : w(e);
      }
      var n = r();
      return typeof n != "function" && b(`unknown function pointer with signature ${t}: ${e}`), n;
    }, cn = (t, e) => {
      var r = At(e, function(n) {
        this.name = e, this.message = n;
        var a = new Error(n).stack;
        a !== void 0 && (this.stack = this.toString() + `
` + a.replace(/^Error(:[^\n]*)?\n/, ""));
      });
      return r.prototype = Object.create(t.prototype), r.prototype.constructor = r, r.prototype.toString = function() {
        return this.message === void 0 ? this.name : `${this.name}: ${this.message}`;
      }, r;
    }, Te, Pe = (t) => {
      var e = Ie(t), r = F(e);
      return X(e), r;
    }, Dt = (t, e) => {
      var r = [], n = {};
      function a(o) {
        if (!n[o] && !Q[o]) {
          if (Pt[o]) {
            Pt[o].forEach(a);
            return;
          }
          r.push(o), n[o] = !0;
        }
      }
      throw e.forEach(a), new Te(`${t}: ` + r.map(Pe).join([", "]));
    }, ln = (t, e, r, n, a, o, u, s, l, d, h, y, $) => {
      h = F(h), o = U(a, o), s && (s = U(u, s)), d && (d = U(l, d)), $ = U(y, $);
      var C = Kr(h);
      $e(C, function() {
        Dt(`Cannot construct ${h} due to unbound types`, [n]);
      }), J([t, e, r], n ? [n] : [], (_) => {
        _ = _[0];
        var I, S;
        n ? (I = _.registeredClass, S = I.instancePrototype) : S = Ot.prototype;
        var x = At(h, function() {
          if (Object.getPrototypeOf(this) !== K)
            throw new at("Use 'new' to construct " + h);
          if (j.constructor_body === void 0)
            throw new at(h + " has no accessible constructor");
          for (var ze = arguments.length, Ft = new Array(ze), jt = 0; jt < ze; jt++)
            Ft[jt] = arguments[jt];
          var Ge = j.constructor_body[Ft.length];
          if (Ge === void 0)
            throw new at(`Tried to invoke ctor of ${h} with invalid number of parameters (${Ft.length}) - expected (${Object.keys(j.constructor_body).toString()}) parameters instead!`);
          return Ge.apply(this, Ft);
        }), K = Object.create(S, {
          constructor: {
            value: x
          }
        });
        x.prototype = K;
        var j = new tn(h, x, K, $, I, o, s, d);
        if (j.baseClass) {
          var q, Mt;
          (Mt = (q = j.baseClass).__derivedClasses) !== null && Mt !== void 0 || (q.__derivedClasses = []), j.baseClass.__derivedClasses.push(j);
        }
        var qa = new St(h, j, !0, !1, !1), Ve = new St(h + "*", j, !1, !1, !1), Ne = new St(h + " const*", j, !1, !0, !1);
        return ge[t] = {
          pointerType: Ve,
          constPointerType: Ne
        }, be(C, x), [qa, Ve, Ne];
      });
    }, qt = (t, e) => {
      for (var r = [], n = 0; n < t; n++)
        r.push(P[e + n * 4 >> 2]);
      return r;
    };
    function fn(t) {
      for (var e = 1; e < t.length; ++e)
        if (t[e] !== null && t[e].destructorFunction === void 0)
          return !0;
      return !1;
    }
    function Zt(t, e, r, n, a, o) {
      var u = e.length;
      u < 2 && b("argTypes array size mismatch! Must at least get return value and 'this' types!");
      var s = e[1] !== null && r !== null, l = fn(e), d = e[0].name !== "void", h = u - 2, y = new Array(h), $ = [], C = [], _ = function() {
        C.length = 0;
        var I;
        $.length = s ? 2 : 1, $[0] = a, s && (I = e[1].toWireType(C, this), $[1] = I);
        for (var S = 0; S < h; ++S)
          y[S] = e[S + 2].toWireType(C, S < 0 || arguments.length <= S ? void 0 : arguments[S]), $.push(y[S]);
        var x = n(...$);
        function K(j) {
          if (l)
            Nt(C);
          else
            for (var q = s ? 1 : 2; q < e.length; q++) {
              var Mt = q === 1 ? I : y[q - 2];
              e[q].destructorFunction !== null && e[q].destructorFunction(Mt);
            }
          if (d)
            return e[0].fromWireType(j);
        }
        return K(x);
      };
      return At(t, _);
    }
    var dn = (t, e, r, n, a, o) => {
      var u = qt(e, r);
      a = U(n, a), J([], [t], (s) => {
        s = s[0];
        var l = `constructor ${s.name}`;
        if (s.registeredClass.constructor_body === void 0 && (s.registeredClass.constructor_body = []), s.registeredClass.constructor_body[e - 1] !== void 0)
          throw new at(`Cannot register multiple constructors with identical number of parameters (${e - 1}) for class '${s.name}'! Overload resolution is currently only performed using the parameter count, not actual type info!`);
        return s.registeredClass.constructor_body[e - 1] = () => {
          Dt(`Cannot construct ${s.name} due to unbound types`, u);
        }, J([], u, (d) => (d.splice(1, 0, null), s.registeredClass.constructor_body[e - 1] = Zt(l, d, null, a, o), [])), [];
      });
    }, _e = (t) => {
      t = t.trim();
      const e = t.indexOf("(");
      return e !== -1 ? t.substr(0, e) : t;
    }, hn = (t, e, r, n, a, o, u, s, l, d) => {
      var h = qt(r, n);
      e = F(e), e = _e(e), o = U(a, o), J([], [t], (y) => {
        y = y[0];
        var $ = `${y.name}.${e}`;
        e.startsWith("@@") && (e = Symbol[e.substring(2)]), s && y.registeredClass.pureVirtualFunctions.push(e);
        function C() {
          Dt(`Cannot call ${$} due to unbound types`, h);
        }
        var _ = y.registeredClass.instancePrototype, I = _[e];
        return I === void 0 || I.overloadTable === void 0 && I.className !== y.name && I.argCount === r - 2 ? (C.argCount = r - 2, C.className = y.name, _[e] = C) : (we(_, e, $), _[e].overloadTable[r - 2] = C), J([], h, (S) => {
          var x = Zt($, S, y, o, u);
          return _[e].overloadTable === void 0 ? (x.argCount = r - 2, _[e] = x) : _[e].overloadTable[r - 2] = x, [];
        }), [];
      });
    }, Yt = [], z = [], Qt = (t) => {
      t > 9 && --z[t + 1] === 0 && (z[t] = void 0, Yt.push(t));
    }, pn = () => z.length / 2 - 5 - Yt.length, yn = () => {
      z.push(0, 1, void 0, 1, null, 1, !0, 1, !1, 1), c.count_emval_handles = pn;
    }, G = {
      toValue: (t) => (t || b("Cannot use deleted val. handle = " + t), z[t]),
      toHandle: (t) => {
        switch (t) {
          case void 0:
            return 2;
          case null:
            return 4;
          case !0:
            return 6;
          case !1:
            return 8;
          default: {
            const e = Yt.pop() || z.length;
            return z[e] = t, z[e + 1] = 1, e;
          }
        }
      }
    }, Ee = {
      name: "emscripten::val",
      fromWireType: (t) => {
        var e = G.toValue(t);
        return Qt(t), e;
      },
      toWireType: (t, e) => G.toHandle(e),
      argPackAdvance: N,
      readValueFromPointer: lt,
      destructorFunction: null
    }, vn = (t) => W(t, Ee), Jt = (t) => {
      if (t === null)
        return "null";
      var e = typeof t;
      return e === "object" || e === "array" || e === "function" ? t.toString() : "" + t;
    }, mn = (t, e) => {
      switch (e) {
        case 4:
          return function(r) {
            return this.fromWireType(oe[r >> 2]);
          };
        case 8:
          return function(r) {
            return this.fromWireType(se[r >> 3]);
          };
        default:
          throw new TypeError(`invalid float width (${e}): ${t}`);
      }
    }, gn = (t, e, r) => {
      e = F(e), W(t, {
        name: e,
        fromWireType: (n) => n,
        toWireType: (n, a) => a,
        argPackAdvance: N,
        readValueFromPointer: mn(e, r),
        destructorFunction: null
      });
    }, wn = (t, e, r, n, a, o, u, s) => {
      var l = qt(e, r);
      t = F(t), t = _e(t), a = U(n, a), $e(t, function() {
        Dt(`Cannot call ${t} due to unbound types`, l);
      }, e - 1), J([], l, (d) => {
        var h = [d[0], null].concat(d.slice(1));
        return be(t, Zt(t, h, null, a, o), e - 1), [];
      });
    }, $n = (t, e, r) => {
      switch (e) {
        case 1:
          return r ? (n) => H[n] : (n) => B[n];
        case 2:
          return r ? (n) => ut[n >> 1] : (n) => gt[n >> 1];
        case 4:
          return r ? (n) => rt[n >> 2] : (n) => P[n >> 2];
        default:
          throw new TypeError(`invalid integer width (${e}): ${t}`);
      }
    }, bn = (t, e, r, n, a) => {
      e = F(e);
      var o = (h) => h;
      if (n === 0) {
        var u = 32 - 8 * r;
        o = (h) => h << u >>> u;
      }
      var s = e.includes("unsigned"), l = (h, y) => {
      }, d;
      s ? d = function(h, y) {
        return l(y, this.name), y >>> 0;
      } : d = function(h, y) {
        return l(y, this.name), y;
      }, W(t, {
        name: e,
        fromWireType: o,
        toWireType: d,
        argPackAdvance: N,
        readValueFromPointer: $n(e, r, n !== 0),
        destructorFunction: null
      });
    }, Cn = (t, e, r) => {
      var n = [Int8Array, Uint8Array, Int16Array, Uint16Array, Int32Array, Uint32Array, Float32Array, Float64Array], a = n[e];
      function o(u) {
        var s = P[u >> 2], l = P[u + 4 >> 2];
        return new a(H.buffer, l, s);
      }
      r = F(r), W(t, {
        name: r,
        fromWireType: o,
        argPackAdvance: N,
        readValueFromPointer: o
      }, {
        ignoreDuplicateRegistrations: !0
      });
    }, Tn = Object.assign({
      optional: !0
    }, Ee), Pn = (t, e) => {
      W(t, Tn);
    }, _n = (t, e, r, n) => {
      if (!(n > 0)) return 0;
      for (var a = r, o = r + n - 1, u = 0; u < t.length; ++u) {
        var s = t.charCodeAt(u);
        if (s >= 55296 && s <= 57343) {
          var l = t.charCodeAt(++u);
          s = 65536 + ((s & 1023) << 10) | l & 1023;
        }
        if (s <= 127) {
          if (r >= o) break;
          e[r++] = s;
        } else if (s <= 2047) {
          if (r + 1 >= o) break;
          e[r++] = 192 | s >> 6, e[r++] = 128 | s & 63;
        } else if (s <= 65535) {
          if (r + 2 >= o) break;
          e[r++] = 224 | s >> 12, e[r++] = 128 | s >> 6 & 63, e[r++] = 128 | s & 63;
        } else {
          if (r + 3 >= o) break;
          e[r++] = 240 | s >> 18, e[r++] = 128 | s >> 12 & 63, e[r++] = 128 | s >> 6 & 63, e[r++] = 128 | s & 63;
        }
      }
      return e[r] = 0, r - a;
    }, dt = (t, e, r) => _n(t, B, e, r), En = (t) => {
      for (var e = 0, r = 0; r < t.length; ++r) {
        var n = t.charCodeAt(r);
        n <= 127 ? e++ : n <= 2047 ? e += 2 : n >= 55296 && n <= 57343 ? (e += 4, ++r) : e += 3;
      }
      return e;
    }, Oe = typeof TextDecoder < "u" ? new TextDecoder() : void 0, Ae = function(t) {
      let e = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : 0, r = arguments.length > 2 && arguments[2] !== void 0 ? arguments[2] : NaN;
      for (var n = e + r, a = e; t[a] && !(a >= n); ) ++a;
      if (a - e > 16 && t.buffer && Oe)
        return Oe.decode(t.subarray(e, a));
      for (var o = ""; e < a; ) {
        var u = t[e++];
        if (!(u & 128)) {
          o += String.fromCharCode(u);
          continue;
        }
        var s = t[e++] & 63;
        if ((u & 224) == 192) {
          o += String.fromCharCode((u & 31) << 6 | s);
          continue;
        }
        var l = t[e++] & 63;
        if ((u & 240) == 224 ? u = (u & 15) << 12 | s << 6 | l : u = (u & 7) << 18 | s << 12 | l << 6 | t[e++] & 63, u < 65536)
          o += String.fromCharCode(u);
        else {
          var d = u - 65536;
          o += String.fromCharCode(55296 | d >> 10, 56320 | d & 1023);
        }
      }
      return o;
    }, On = (t, e) => t ? Ae(B, t, e) : "", An = (t, e) => {
      e = F(e), W(t, {
        name: e,
        fromWireType(r) {
          for (var n = P[r >> 2], a = r + 4, o, s, u = a, s = 0; s <= n; ++s) {
            var l = a + s;
            if (s == n || B[l] == 0) {
              var d = l - u, h = On(u, d);
              o === void 0 ? o = h : (o += "\0", o += h), u = l + 1;
            }
          }
          return X(r), o;
        },
        toWireType(r, n) {
          n instanceof ArrayBuffer && (n = new Uint8Array(n));
          var a, o = typeof n == "string";
          o || n instanceof Uint8Array || n instanceof Uint8ClampedArray || n instanceof Int8Array || b("Cannot pass non-string to std::string"), o ? a = En(n) : a = n.length;
          var u = ee(4 + a + 1), s = u + 4;
          if (P[u >> 2] = a, o)
            dt(n, s, a + 1);
          else if (o)
            for (var l = 0; l < a; ++l) {
              var d = n.charCodeAt(l);
              d > 255 && (X(s), b("String has UTF-16 code units that do not fit in 8 bits")), B[s + l] = d;
            }
          else
            for (var l = 0; l < a; ++l)
              B[s + l] = n[l];
          return r !== null && r.push(X, u), u;
        },
        argPackAdvance: N,
        readValueFromPointer: lt,
        destructorFunction(r) {
          X(r);
        }
      });
    }, Se = typeof TextDecoder < "u" ? new TextDecoder("utf-16le") : void 0, Sn = (t, e) => {
      for (var r = t, n = r >> 1, a = n + e / 2; !(n >= a) && gt[n]; ) ++n;
      if (r = n << 1, r - t > 32 && Se) return Se.decode(B.subarray(t, r));
      for (var o = "", u = 0; !(u >= e / 2); ++u) {
        var s = ut[t + u * 2 >> 1];
        if (s == 0) break;
        o += String.fromCharCode(s);
      }
      return o;
    }, xn = (t, e, r) => {
      var n;
      if ((n = r) !== null && n !== void 0 || (r = 2147483647), r < 2) return 0;
      r -= 2;
      for (var a = e, o = r < t.length * 2 ? r / 2 : t.length, u = 0; u < o; ++u) {
        var s = t.charCodeAt(u);
        ut[e >> 1] = s, e += 2;
      }
      return ut[e >> 1] = 0, e - a;
    }, Dn = (t) => t.length * 2, In = (t, e) => {
      for (var r = 0, n = ""; !(r >= e / 4); ) {
        var a = rt[t + r * 4 >> 2];
        if (a == 0) break;
        if (++r, a >= 65536) {
          var o = a - 65536;
          n += String.fromCharCode(55296 | o >> 10, 56320 | o & 1023);
        } else
          n += String.fromCharCode(a);
      }
      return n;
    }, Mn = (t, e, r) => {
      var n;
      if ((n = r) !== null && n !== void 0 || (r = 2147483647), r < 4) return 0;
      for (var a = e, o = a + r - 4, u = 0; u < t.length; ++u) {
        var s = t.charCodeAt(u);
        if (s >= 55296 && s <= 57343) {
          var l = t.charCodeAt(++u);
          s = 65536 + ((s & 1023) << 10) | l & 1023;
        }
        if (rt[e >> 2] = s, e += 4, e + 4 > o) break;
      }
      return rt[e >> 2] = 0, e - a;
    }, Fn = (t) => {
      for (var e = 0, r = 0; r < t.length; ++r) {
        var n = t.charCodeAt(r);
        n >= 55296 && n <= 57343 && ++r, e += 4;
      }
      return e;
    }, jn = (t, e, r) => {
      r = F(r);
      var n, a, o, u;
      e === 2 ? (n = Sn, a = xn, u = Dn, o = (s) => gt[s >> 1]) : e === 4 && (n = In, a = Mn, u = Fn, o = (s) => P[s >> 2]), W(t, {
        name: r,
        fromWireType: (s) => {
          for (var l = P[s >> 2], d, h = s + 4, y = 0; y <= l; ++y) {
            var $ = s + 4 + y * e;
            if (y == l || o($) == 0) {
              var C = $ - h, _ = n(h, C);
              d === void 0 ? d = _ : (d += "\0", d += _), h = $ + e;
            }
          }
          return X(s), d;
        },
        toWireType: (s, l) => {
          typeof l != "string" && b(`Cannot pass non-string to C++ string type ${r}`);
          var d = u(l), h = ee(4 + d + e);
          return P[h >> 2] = d / e, a(l, h + 4, d + e), s !== null && s.push(X, h), h;
        },
        argPackAdvance: N,
        readValueFromPointer: lt,
        destructorFunction(s) {
          X(s);
        }
      });
    }, Rn = (t, e, r, n, a, o) => {
      Tt[t] = {
        name: F(e),
        rawConstructor: U(r, n),
        rawDestructor: U(a, o),
        fields: []
      };
    }, Ln = (t, e, r, n, a, o, u, s, l, d) => {
      Tt[t].fields.push({
        fieldName: F(e),
        getterReturnType: r,
        getter: U(n, a),
        getterContext: o,
        setterArgumentType: u,
        setter: U(s, l),
        setterContext: d
      });
    }, Bn = (t, e) => {
      e = F(e), W(t, {
        isVoid: !0,
        name: e,
        argPackAdvance: 0,
        fromWireType: () => {
        },
        toWireType: (r, n) => {
        }
      });
    }, Un = (t, e, r) => B.copyWithin(t, e, e + r), Kt = [], Wn = (t, e, r, n) => (t = Kt[t], e = G.toValue(e), t(null, e, r, n)), kn = {}, Hn = (t) => {
      var e = kn[t];
      return e === void 0 ? F(t) : e;
    }, xe = () => {
      if (typeof globalThis == "object")
        return globalThis;
      function t(e) {
        e.$$$embind_global$$$ = e;
        var r = typeof $$$embind_global$$$ == "object" && e.$$$embind_global$$$ == e;
        return r || delete e.$$$embind_global$$$, r;
      }
      if (typeof $$$embind_global$$$ == "object" || (typeof global == "object" && t(global) ? $$$embind_global$$$ = global : typeof self == "object" && t(self) && ($$$embind_global$$$ = self), typeof $$$embind_global$$$ == "object"))
        return $$$embind_global$$$;
      throw Error("unable to get global object.");
    }, Vn = (t) => t === 0 ? G.toHandle(xe()) : (t = Hn(t), G.toHandle(xe()[t])), Nn = (t) => {
      var e = Kt.length;
      return Kt.push(t), e;
    }, De = (t, e) => {
      var r = Q[t];
      return r === void 0 && b(`${e} has unknown type ${Pe(t)}`), r;
    }, zn = (t, e) => {
      for (var r = new Array(t), n = 0; n < t; ++n)
        r[n] = De(P[e + n * 4 >> 2], "parameter " + n);
      return r;
    }, Gn = Reflect.construct, Xn = (t, e, r) => {
      var n = [], a = t.toWireType(n, r);
      return n.length && (P[e >> 2] = G.toHandle(n)), a;
    }, qn = (t, e, r) => {
      var n = zn(t, e), a = n.shift();
      t--;
      var o = new Array(t), u = (l, d, h, y) => {
        for (var $ = 0, C = 0; C < t; ++C)
          o[C] = n[C].readValueFromPointer(y + $), $ += n[C].argPackAdvance;
        var _ = r === 1 ? Gn(d, o) : d.apply(l, o);
        return Xn(a, h, _);
      }, s = `methodCaller<(${n.map((l) => l.name).join(", ")}) => ${a.name}>`;
      return Nn(At(s, u));
    }, Zn = (t) => {
      t > 9 && (z[t + 1] += 1);
    }, Yn = (t) => {
      var e = G.toValue(t);
      Nt(e), Qt(t);
    }, Qn = (t, e) => {
      t = De(t, "_emval_take_value");
      var r = t.readValueFromPointer(e);
      return G.toHandle(r);
    }, Jn = (t, e, r, n) => {
      var a = (/* @__PURE__ */ new Date()).getFullYear(), o = new Date(a, 0, 1), u = new Date(a, 6, 1), s = o.getTimezoneOffset(), l = u.getTimezoneOffset(), d = Math.max(s, l);
      P[t >> 2] = d * 60, rt[e >> 2] = +(s != l);
      var h = (C) => {
        var _ = C >= 0 ? "-" : "+", I = Math.abs(C), S = String(Math.floor(I / 60)).padStart(2, "0"), x = String(I % 60).padStart(2, "0");
        return `UTC${_}${S}${x}`;
      }, y = h(s), $ = h(l);
      l < s ? (dt(y, r, 17), dt($, n, 17)) : (dt(y, n, 17), dt($, r, 17));
    }, Kn = () => 2147483648, ta = (t, e) => Math.ceil(t / e) * e, ea = (t) => {
      var e = mt.buffer, r = (t - e.byteLength + 65535) / 65536 | 0;
      try {
        return mt.grow(r), ue(), 1;
      } catch {
      }
    }, ra = (t) => {
      var e = B.length;
      t >>>= 0;
      var r = Kn();
      if (t > r)
        return !1;
      for (var n = 1; n <= 4; n *= 2) {
        var a = e * (1 + 0.2 / n);
        a = Math.min(a, t + 100663296);
        var o = Math.min(r, ta(Math.max(t, a), 65536)), u = ea(o);
        if (u)
          return !0;
      }
      return !1;
    }, te = {}, na = () => tt || "./this.program", ht = () => {
      if (!ht.strings) {
        var t = (typeof navigator == "object" && navigator.languages && navigator.languages[0] || "C").replace("-", "_") + ".UTF-8", e = {
          USER: "web_user",
          LOGNAME: "web_user",
          PATH: "/",
          PWD: "/",
          HOME: "/home/web_user",
          LANG: t,
          _: na()
        };
        for (var r in te)
          te[r] === void 0 ? delete e[r] : e[r] = te[r];
        var n = [];
        for (var r in e)
          n.push(`${r}=${e[r]}`);
        ht.strings = n;
      }
      return ht.strings;
    }, aa = (t, e) => {
      for (var r = 0; r < t.length; ++r)
        H[e++] = t.charCodeAt(r);
      H[e] = 0;
    }, ia = (t, e) => {
      var r = 0;
      return ht().forEach((n, a) => {
        var o = e + r;
        P[t + a * 4 >> 2] = o, aa(n, o), r += n.length + 1;
      }), 0;
    }, oa = (t, e) => {
      var r = ht();
      P[t >> 2] = r.length;
      var n = 0;
      return r.forEach((a) => n += a.length + 1), P[e >> 2] = n, 0;
    }, sa = (t) => 52;
    function ua(t, e, r, n, a) {
      return 70;
    }
    var ca = [null, [], []], la = (t, e) => {
      var r = ca[t];
      e === 0 || e === 10 ? ((t === 1 ? dr : et)(Ae(r)), r.length = 0) : r.push(e);
    }, fa = (t, e, r, n) => {
      for (var a = 0, o = 0; o < r; o++) {
        var u = P[e >> 2], s = P[e + 4 >> 2];
        e += 8;
        for (var l = 0; l < s; l++)
          la(t, B[u + l]);
        a += s;
      }
      return P[n >> 2] = a, 0;
    }, da = (t) => t;
    he = c.InternalError = class extends Error {
      constructor(t) {
        super(t), this.name = "InternalError";
      }
    }, kr(), at = c.BindingError = class extends Error {
      constructor(t) {
        super(t), this.name = "BindingError";
      }
    }, Yr(), an(), Te = c.UnboundTypeError = cn(Error, "UnboundTypeError"), yn();
    var ha = {
      s: Sr,
      w: xr,
      a: Ir,
      j: Mr,
      l: Fr,
      M: jr,
      p: Rr,
      ca: Lr,
      d: Dr,
      Z: Br,
      sa: Ur,
      Y: Wr,
      na: Vr,
      qa: ln,
      pa: dn,
      D: hn,
      la: vn,
      Q: gn,
      R: wn,
      x: bn,
      t: Cn,
      ra: Pn,
      ma: An,
      N: jn,
      J: Rn,
      ta: Ln,
      oa: Bn,
      fa: Un,
      U: Wn,
      ua: Qt,
      wa: Vn,
      _: qn,
      S: Zn,
      va: Yn,
      ka: Qn,
      $: Jn,
      da: ra,
      aa: ia,
      ba: oa,
      ea: sa,
      W: ua,
      P: fa,
      H: Fa,
      B: Ra,
      T: wa,
      O: Va,
      q: xa,
      b: ya,
      C: Ma,
      ha: Ba,
      c: ma,
      ga: Ua,
      h: ga,
      i: Pa,
      r: _a,
      L: Ia,
      v: Oa,
      G: ka,
      I: Da,
      z: La,
      F: Na,
      X: Ga,
      V: Xa,
      k: ba,
      f: $a,
      e: va,
      g: pa,
      K: Ha,
      m: Ta,
      ia: ja,
      o: Ea,
      u: Aa,
      ja: Sa,
      A: Wa,
      n: Ca,
      E: za,
      y: da
    }, E;
    Ar();
    var Ie = (t) => (Ie = E.za)(t), X = c._free = (t) => (X = c._free = E.Aa)(t), ee = c._malloc = (t) => (ee = c._malloc = E.Ca)(t), Me = (t) => (Me = E.Da)(t), v = (t, e) => (v = E.Ea)(t, e), Fe = (t) => (Fe = E.Fa)(t), je = (t) => (je = E.Ga)(t), Re = () => (Re = E.Ha)(), Le = (t) => (Le = E.Ia)(t), Be = (t) => (Be = E.Ja)(t), Ue = (t, e, r) => (Ue = E.Ka)(t, e, r);
    c.dynCall_viijii = (t, e, r, n, a, o, u) => (c.dynCall_viijii = E.La)(t, e, r, n, a, o, u);
    var We = c.dynCall_jiii = (t, e, r, n) => (We = c.dynCall_jiii = E.Ma)(t, e, r, n);
    c.dynCall_jiji = (t, e, r, n, a) => (c.dynCall_jiji = E.Na)(t, e, r, n, a);
    var ke = c.dynCall_jiiii = (t, e, r, n, a) => (ke = c.dynCall_jiiii = E.Oa)(t, e, r, n, a);
    c.dynCall_iiiiij = (t, e, r, n, a, o, u) => (c.dynCall_iiiiij = E.Pa)(t, e, r, n, a, o, u), c.dynCall_iiiiijj = (t, e, r, n, a, o, u, s, l) => (c.dynCall_iiiiijj = E.Qa)(t, e, r, n, a, o, u, s, l), c.dynCall_iiiiiijj = (t, e, r, n, a, o, u, s, l, d) => (c.dynCall_iiiiiijj = E.Ra)(t, e, r, n, a, o, u, s, l, d);
    function pa(t, e, r, n) {
      var a = g();
      try {
        w(t)(e, r, n);
      } catch (o) {
        if (m(a), o !== o + 0) throw o;
        v(1, 0);
      }
    }
    function ya(t, e) {
      var r = g();
      try {
        return w(t)(e);
      } catch (n) {
        if (m(r), n !== n + 0) throw n;
        v(1, 0);
      }
    }
    function va(t, e, r) {
      var n = g();
      try {
        w(t)(e, r);
      } catch (a) {
        if (m(n), a !== a + 0) throw a;
        v(1, 0);
      }
    }
    function ma(t, e, r) {
      var n = g();
      try {
        return w(t)(e, r);
      } catch (a) {
        if (m(n), a !== a + 0) throw a;
        v(1, 0);
      }
    }
    function ga(t, e, r, n) {
      var a = g();
      try {
        return w(t)(e, r, n);
      } catch (o) {
        if (m(a), o !== o + 0) throw o;
        v(1, 0);
      }
    }
    function wa(t, e, r, n, a) {
      var o = g();
      try {
        return w(t)(e, r, n, a);
      } catch (u) {
        if (m(o), u !== u + 0) throw u;
        v(1, 0);
      }
    }
    function $a(t, e) {
      var r = g();
      try {
        w(t)(e);
      } catch (n) {
        if (m(r), n !== n + 0) throw n;
        v(1, 0);
      }
    }
    function ba(t) {
      var e = g();
      try {
        w(t)();
      } catch (r) {
        if (m(e), r !== r + 0) throw r;
        v(1, 0);
      }
    }
    function Ca(t, e, r, n, a, o, u, s, l, d, h) {
      var y = g();
      try {
        w(t)(e, r, n, a, o, u, s, l, d, h);
      } catch ($) {
        if (m(y), $ !== $ + 0) throw $;
        v(1, 0);
      }
    }
    function Ta(t, e, r, n, a) {
      var o = g();
      try {
        w(t)(e, r, n, a);
      } catch (u) {
        if (m(o), u !== u + 0) throw u;
        v(1, 0);
      }
    }
    function Pa(t, e, r, n, a) {
      var o = g();
      try {
        return w(t)(e, r, n, a);
      } catch (u) {
        if (m(o), u !== u + 0) throw u;
        v(1, 0);
      }
    }
    function _a(t, e, r, n, a, o) {
      var u = g();
      try {
        return w(t)(e, r, n, a, o);
      } catch (s) {
        if (m(u), s !== s + 0) throw s;
        v(1, 0);
      }
    }
    function Ea(t, e, r, n, a, o) {
      var u = g();
      try {
        w(t)(e, r, n, a, o);
      } catch (s) {
        if (m(u), s !== s + 0) throw s;
        v(1, 0);
      }
    }
    function Oa(t, e, r, n, a, o, u) {
      var s = g();
      try {
        return w(t)(e, r, n, a, o, u);
      } catch (l) {
        if (m(s), l !== l + 0) throw l;
        v(1, 0);
      }
    }
    function Aa(t, e, r, n, a, o, u, s) {
      var l = g();
      try {
        w(t)(e, r, n, a, o, u, s);
      } catch (d) {
        if (m(l), d !== d + 0) throw d;
        v(1, 0);
      }
    }
    function Sa(t, e, r, n, a, o, u, s, l) {
      var d = g();
      try {
        w(t)(e, r, n, a, o, u, s, l);
      } catch (h) {
        if (m(d), h !== h + 0) throw h;
        v(1, 0);
      }
    }
    function xa(t) {
      var e = g();
      try {
        return w(t)();
      } catch (r) {
        if (m(e), r !== r + 0) throw r;
        v(1, 0);
      }
    }
    function Da(t, e, r, n, a, o, u, s, l) {
      var d = g();
      try {
        return w(t)(e, r, n, a, o, u, s, l);
      } catch (h) {
        if (m(d), h !== h + 0) throw h;
        v(1, 0);
      }
    }
    function Ia(t, e, r, n, a, o, u) {
      var s = g();
      try {
        return w(t)(e, r, n, a, o, u);
      } catch (l) {
        if (m(s), l !== l + 0) throw l;
        v(1, 0);
      }
    }
    function Ma(t, e, r, n) {
      var a = g();
      try {
        return w(t)(e, r, n);
      } catch (o) {
        if (m(a), o !== o + 0) throw o;
        v(1, 0);
      }
    }
    function Fa(t, e, r, n) {
      var a = g();
      try {
        return w(t)(e, r, n);
      } catch (o) {
        if (m(a), o !== o + 0) throw o;
        v(1, 0);
      }
    }
    function ja(t, e, r, n, a, o, u, s) {
      var l = g();
      try {
        w(t)(e, r, n, a, o, u, s);
      } catch (d) {
        if (m(l), d !== d + 0) throw d;
        v(1, 0);
      }
    }
    function Ra(t, e, r, n, a, o) {
      var u = g();
      try {
        return w(t)(e, r, n, a, o);
      } catch (s) {
        if (m(u), s !== s + 0) throw s;
        v(1, 0);
      }
    }
    function La(t, e, r, n, a, o, u, s, l, d) {
      var h = g();
      try {
        return w(t)(e, r, n, a, o, u, s, l, d);
      } catch (y) {
        if (m(h), y !== y + 0) throw y;
        v(1, 0);
      }
    }
    function Ba(t, e, r) {
      var n = g();
      try {
        return w(t)(e, r);
      } catch (a) {
        if (m(n), a !== a + 0) throw a;
        v(1, 0);
      }
    }
    function Ua(t, e, r, n, a) {
      var o = g();
      try {
        return w(t)(e, r, n, a);
      } catch (u) {
        if (m(o), u !== u + 0) throw u;
        v(1, 0);
      }
    }
    function Wa(t, e, r, n, a, o, u, s, l, d) {
      var h = g();
      try {
        w(t)(e, r, n, a, o, u, s, l, d);
      } catch (y) {
        if (m(h), y !== y + 0) throw y;
        v(1, 0);
      }
    }
    function ka(t, e, r, n, a, o, u, s) {
      var l = g();
      try {
        return w(t)(e, r, n, a, o, u, s);
      } catch (d) {
        if (m(l), d !== d + 0) throw d;
        v(1, 0);
      }
    }
    function Ha(t, e, r, n, a, o, u) {
      var s = g();
      try {
        w(t)(e, r, n, a, o, u);
      } catch (l) {
        if (m(s), l !== l + 0) throw l;
        v(1, 0);
      }
    }
    function Va(t, e, r, n) {
      var a = g();
      try {
        return w(t)(e, r, n);
      } catch (o) {
        if (m(a), o !== o + 0) throw o;
        v(1, 0);
      }
    }
    function Na(t, e, r, n, a, o, u, s, l, d, h, y) {
      var $ = g();
      try {
        return w(t)(e, r, n, a, o, u, s, l, d, h, y);
      } catch (C) {
        if (m($), C !== C + 0) throw C;
        v(1, 0);
      }
    }
    function za(t, e, r, n, a, o, u, s, l, d, h, y, $, C, _, I) {
      var S = g();
      try {
        w(t)(e, r, n, a, o, u, s, l, d, h, y, $, C, _, I);
      } catch (x) {
        if (m(S), x !== x + 0) throw x;
        v(1, 0);
      }
    }
    function Ga(t, e, r, n) {
      var a = g();
      try {
        return We(t, e, r, n);
      } catch (o) {
        if (m(a), o !== o + 0) throw o;
        v(1, 0);
      }
    }
    function Xa(t, e, r, n, a) {
      var o = g();
      try {
        return ke(t, e, r, n, a);
      } catch (u) {
        if (m(o), u !== u + 0) throw u;
        v(1, 0);
      }
    }
    var It;
    ct = function t() {
      It || He(), It || (ct = t);
    };
    function He() {
      if (Y > 0 || (hr(), Y > 0))
        return;
      function t() {
        var e;
        It || (It = !0, c.calledRun = !0, !ie && (pr(), O(c), (e = c.onRuntimeInitialized) === null || e === void 0 || e.call(c), yr()));
      }
      c.setStatus ? (c.setStatus("Running..."), setTimeout(() => {
        setTimeout(() => c.setStatus(""), 1), t();
      }, 1)) : t();
    }
    if (c.preInit)
      for (typeof c.preInit == "function" && (c.preInit = [c.preInit]); c.preInit.length > 0; )
        c.preInit.pop()();
    return He(), T = R, T;
  };
})();
function rr(i) {
  return er(ae, i);
}
function Mi() {
  return fi(ae);
}
function Fi(i) {
  rr({
    overrides: i,
    equalityFn: Object.is,
    fireImmediately: !1
  });
}
async function hi(i, f) {
  return di(ae, i, f);
}
const ji = "5bb3ce7aeedc314e24afb873bebcf366a01c27a00b21cdf2fa98b819c005c16b", nr = [
  ["aztec", "Aztec"],
  ["code_128", "Code128"],
  ["code_39", "Code39"],
  ["code_93", "Code93"],
  ["codabar", "Codabar"],
  ["databar", "DataBar"],
  ["databar_expanded", "DataBarExpanded"],
  ["databar_limited", "DataBarLimited"],
  ["data_matrix", "DataMatrix"],
  ["dx_film_edge", "DXFilmEdge"],
  ["ean_13", "EAN-13"],
  ["ean_8", "EAN-8"],
  ["itf", "ITF"],
  ["maxi_code", "MaxiCode"],
  ["micro_qr_code", "MicroQRCode"],
  ["pdf417", "PDF417"],
  ["qr_code", "QRCode"],
  ["rm_qr_code", "rMQRCode"],
  ["upc_a", "UPC-A"],
  ["upc_e", "UPC-E"],
  ["linear_codes", "Linear-Codes"],
  ["matrix_codes", "Matrix-Codes"],
  ["any", "Any"]
], pi = [...nr, ["unknown"]].map((i) => i[0]), ne = new Map(
  nr
);
function yi(i) {
  for (const [f, p] of ne)
    if (i === p)
      return f;
  return "unknown";
}
function vi(i) {
  if (ar(i))
    return {
      width: i.naturalWidth,
      height: i.naturalHeight
    };
  if (ir(i))
    return {
      width: i.width.baseVal.value,
      height: i.height.baseVal.value
    };
  if (or(i))
    return {
      width: i.videoWidth,
      height: i.videoHeight
    };
  if (ur(i))
    return {
      width: i.width,
      height: i.height
    };
  if (lr(i))
    return {
      width: i.displayWidth,
      height: i.displayHeight
    };
  if (sr(i))
    return {
      width: i.width,
      height: i.height
    };
  if (cr(i))
    return {
      width: i.width,
      height: i.height
    };
  throw new TypeError(
    "The provided value is not of type '(Blob or HTMLCanvasElement or HTMLImageElement or HTMLVideoElement or ImageBitmap or ImageData or OffscreenCanvas or SVGImageElement or VideoFrame)'."
  );
}
function ar(i) {
  var f, p;
  try {
    return i instanceof ((p = (f = i == null ? void 0 : i.ownerDocument) == null ? void 0 : f.defaultView) == null ? void 0 : p.HTMLImageElement);
  } catch {
    return !1;
  }
}
function ir(i) {
  var f, p;
  try {
    return i instanceof ((p = (f = i == null ? void 0 : i.ownerDocument) == null ? void 0 : f.defaultView) == null ? void 0 : p.SVGImageElement);
  } catch {
    return !1;
  }
}
function or(i) {
  var f, p;
  try {
    return i instanceof ((p = (f = i == null ? void 0 : i.ownerDocument) == null ? void 0 : f.defaultView) == null ? void 0 : p.HTMLVideoElement);
  } catch {
    return !1;
  }
}
function sr(i) {
  var f, p;
  try {
    return i instanceof ((p = (f = i == null ? void 0 : i.ownerDocument) == null ? void 0 : f.defaultView) == null ? void 0 : p.HTMLCanvasElement);
  } catch {
    return !1;
  }
}
function ur(i) {
  try {
    return i instanceof ImageBitmap || Object.prototype.toString.call(i) === "[object ImageBitmap]";
  } catch {
    return !1;
  }
}
function cr(i) {
  try {
    return i instanceof OffscreenCanvas || Object.prototype.toString.call(i) === "[object OffscreenCanvas]";
  } catch {
    return !1;
  }
}
function lr(i) {
  try {
    return i instanceof VideoFrame || Object.prototype.toString.call(i) === "[object VideoFrame]";
  } catch {
    return !1;
  }
}
function mi(i) {
  try {
    return i instanceof Blob || Object.prototype.toString.call(i) === "[object Blob]";
  } catch {
    return !1;
  }
}
function gi(i) {
  try {
    return i instanceof ImageData || Object.prototype.toString.call(i) === "[object ImageData]";
  } catch {
    return !1;
  }
}
function wi(i, f) {
  try {
    const p = new OffscreenCanvas(i, f);
    if (p.getContext("2d") instanceof OffscreenCanvasRenderingContext2D)
      return p;
    throw void 0;
  } catch {
    const p = document.createElement("canvas");
    return p.width = i, p.height = f, p;
  }
}
async function fr(i) {
  if (ar(i) && !await Ti(i))
    throw new DOMException(
      "Failed to load or decode HTMLImageElement.",
      "InvalidStateError"
    );
  if (ir(i) && !await Pi(i))
    throw new DOMException(
      "Failed to load or decode SVGImageElement.",
      "InvalidStateError"
    );
  if (lr(i) && _i(i))
    throw new DOMException("VideoFrame is closed.", "InvalidStateError");
  if (or(i) && (i.readyState === 0 || i.readyState === 1))
    throw new DOMException("Invalid element or state.", "InvalidStateError");
  if (ur(i) && Oi(i))
    throw new DOMException(
      "The image source is detached.",
      "InvalidStateError"
    );
  const { width: f, height: p } = vi(i);
  if (f === 0 || p === 0)
    return null;
  const c = wi(f, p).getContext("2d");
  c.drawImage(i, 0, 0);
  try {
    return c.getImageData(0, 0, f, p);
  } catch {
    throw new DOMException("Source would taint origin.", "SecurityError");
  }
}
async function $i(i) {
  let f;
  try {
    f = await createImageBitmap(i);
  } catch {
    try {
      if (globalThis.Image) {
        f = new Image();
        let c = "";
        try {
          c = URL.createObjectURL(i), f.src = c, await f.decode();
        } finally {
          URL.revokeObjectURL(c);
        }
      } else
        return i;
    } catch {
      throw new DOMException(
        "Failed to load or decode Blob.",
        "InvalidStateError"
      );
    }
  }
  return await fr(f);
}
function bi(i) {
  const { width: f, height: p } = i;
  if (f === 0 || p === 0)
    return null;
  const T = i.getContext("2d");
  try {
    return T.getImageData(0, 0, f, p);
  } catch {
    throw new DOMException("Source would taint origin.", "SecurityError");
  }
}
async function Ci(i) {
  if (mi(i))
    return await $i(i);
  if (gi(i)) {
    if (Ei(i))
      throw new DOMException(
        "The image data has been detached.",
        "InvalidStateError"
      );
    return i;
  }
  return sr(i) || cr(i) ? bi(i) : await fr(i);
}
async function Ti(i) {
  try {
    return await i.decode(), !0;
  } catch {
    return !1;
  }
}
async function Pi(i) {
  var f;
  try {
    return await ((f = i.decode) == null ? void 0 : f.call(i)), !0;
  } catch {
    return !1;
  }
}
function _i(i) {
  return i.format === null;
}
function Ei(i) {
  return i.data.buffer.byteLength === 0;
}
function Oi(i) {
  return i.width === 0 && i.height === 0;
}
function tr(i, f) {
  return Ai(i) ? new DOMException(`${f}: ${i.message}`, i.name) : Si(i) ? new i.constructor(`${f}: ${i.message}`) : new Error(`${f}: ${i}`);
}
function Ai(i) {
  return i instanceof DOMException || Object.prototype.toString.call(i) === "[object DOMException]";
}
function Si(i) {
  return i instanceof Error || Object.prototype.toString.call(i) === "[object Error]";
}
var pt;
class Ri {
  constructor(f = {}) {
    Ye(this, pt);
    var p;
    try {
      const T = (p = f == null ? void 0 : f.formats) == null ? void 0 : p.filter(
        (c) => c !== "unknown"
      );
      if ((T == null ? void 0 : T.length) === 0)
        throw new TypeError("Hint option provided, but is empty.");
      for (const c of T != null ? T : [])
        if (!ne.has(c))
          throw new TypeError(
            `Failed to read the 'formats' property from 'BarcodeDetectorOptions': The provided value '${c}' is not a valid enum value of type BarcodeFormat.`
          );
      Qe(this, pt, T != null ? T : []), rr({ fireImmediately: !0 }).catch(() => {
      });
    } catch (T) {
      throw tr(
        T,
        "Failed to construct 'BarcodeDetector'"
      );
    }
  }
  static async getSupportedFormats() {
    return pi.filter((f) => f !== "unknown");
  }
  async detect(f) {
    try {
      const p = await Ci(f);
      if (p === null)
        return [];
      let T;
      const c = {
        tryCode39ExtendedMode: !1,
        textMode: "Plain",
        formats: Ze(this, pt).map((O) => ne.get(O))
      };
      try {
        T = await hi(p, c);
      } catch (O) {
        throw console.error(O), new DOMException(
          "Barcode detection service unavailable.",
          "NotSupportedError"
        );
      }
      return T.map((O) => {
        const {
          topLeft: { x: D, y: R },
          topRight: { x: A, y: M },
          bottomLeft: { x: k, y: Z },
          bottomRight: { x: tt, y: L }
        } = O.position, yt = Math.min(D, A, k, tt), it = Math.min(R, M, Z, L), ot = Math.max(D, A, k, tt), st = Math.max(R, M, Z, L);
        return {
          boundingBox: new DOMRectReadOnly(
            yt,
            it,
            ot - yt,
            st - it
          ),
          rawValue: O.text,
          format: yi(O.format),
          cornerPoints: [
            {
              x: D,
              y: R
            },
            {
              x: A,
              y: M
            },
            {
              x: tt,
              y: L
            },
            {
              x: k,
              y: Z
            }
          ]
        };
      });
    } catch (p) {
      throw tr(
        p,
        "Failed to execute 'detect' on 'BarcodeDetector'"
      );
    }
  }
}
pt = new WeakMap();
export {
  Ri as BarcodeDetector,
  Ii as ZXING_CPP_COMMIT,
  ji as ZXING_WASM_SHA256,
  Di as ZXING_WASM_VERSION,
  rr as prepareZXingModule,
  Mi as purgeZXingModule,
  Fi as setZXingModuleOverrides
};
