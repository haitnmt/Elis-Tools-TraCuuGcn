<#macro registrationLayout bodyClass="" displayInfo=false displayMessage=true displayRequiredFields=false>
<!DOCTYPE html>
<html class="${properties.kcHtmlClass!}">

<head>
    <meta charset="utf-8">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="robots" content="noindex, nofollow">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=5.0, minimum-scale=1.0">

    <title>${msg("loginTitle",(realm.displayName!''))}</title>
    <meta name="description" content="${msg("loginTitle",(realm.displayName!''))}">
    
    <link rel="icon" href="${url.resourcesPath}/img/favicon.ico" />
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    
    <#if properties.styles?has_content>
        <#list properties.styles?split(',') as style>
            <link href="${url.resourcesPath}/${style}" rel="stylesheet" />
        </#list>
    </#if>
    
    <#if properties.scripts?has_content>
        <#list properties.scripts?split(',') as script>
            <script src="${url.resourcesPath}/${script}" type="text/javascript"></script>
        </#list>
    </#if>
</head>

<body class="${properties.kcBodyClass!}">
    <div class="container">
        <div class="login-container">
            <div class="header">
                <div id="kc-header-wrapper" class="${properties.kcHeaderWrapperClass!}">
                    <div class="logo">
                        <img src="${url.resourcesPath}/img/logo.png" alt="Logo">
                        <h1>${kcSanitize(msg("loginTitleHtml",(realm.displayNameHtml!'')))?no_esc}</h1>
                    </div>
                    <div class="language-selector">
                        <#if realm.internationalizationEnabled  && locale.supported?size gt 1>
                            <div class="dropdown">
                                <button class="dropdown-toggle">
                                    ${locale.current} <i class="fa fa-caret-down"></i>
                                </button>
                                <div class="dropdown-menu">
                                    <#list locale.supported as l>
                                        <a href="${l.url}">${l.label}</a>
                                    </#list>
                                </div>
                            </div>
                        </#if>
                    </div>
                </div>
            </div>

            <div class="main-content">
                <div class="card">
                    <div class="card-header">
                        <h2><#nested "header"></h2>
                    </div>
                    <div class="card-body">
                        <#if displayMessage && message?has_content && (message.type != 'warning' || !isAppInitiatedAction??)>
                            <div class="alert alert-${message.type}">
                                <span class="message-icon">
                                    <#if message.type = 'success'><i class="fa fa-check-circle"></i></#if>
                                    <#if message.type = 'warning'><i class="fa fa-exclamation-triangle"></i></#if>
                                    <#if message.type = 'error'><i class="fa fa-times-circle"></i></#if>
                                    <#if message.type = 'info'><i class="fa fa-info-circle"></i></#if>
                                </span>
                                <span class="message-text">${kcSanitize(message.summary)?no_esc}</span>
                            </div>
                        </#if>

                        <#nested "form">

                        <#if displayInfo>
                            <div class="info-text">
                                <#nested "info">
                            </div>
                        </#if>
                    </div>
                </div>
            </div>

            <div class="footer">
                <div class="footer-content">
                    <#if realm.internationalizationEnabled>
                        <div class="locale-container">
                            <#list locale.supported as l>
                                <a href="${l.url}">${l.label}</a>
                                <#if l_has_next> | </#if>
                            </#list>
                        </div>
                    </#if>
                    <div class="copyright">
                        &copy; ${.now?string('yyyy')} - Keycloak Việt Nam. Tất cả các quyền được bảo lưu.
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
</#macro>
