<#import "template.ftl" as layout>
<@layout.registrationLayout displayMessage=!messagesPerField.existsError('username','password') displayInfo=realm.password && realm.registrationAllowed && !registrationDisabled??; section>
    <#if section = "header">
        ${msg("loginAccountTitle")}
    <#elseif section = "form">
        <div id="kc-form">
            <div id="kc-form-wrapper">
                <#if realm.password>
                    <form id="kc-form-login" onsubmit="login.disabled = true; return true;" action="${url.loginAction}" method="post">
                        <div class="form-group">
                            <label for="username" class="form-label">${msg("username")}</label>
                            <input tabindex="1" id="username" class="form-control" name="username" value="${(login.username!'')}" type="text" autofocus autocomplete="off"
                                   aria-invalid="<#if messagesPerField.existsError('username','password')>true</#if>"
                            />
                            <#if messagesPerField.existsError('username','password')>
                                <span id="input-error" class="form-error" aria-live="polite">
                                    ${kcSanitize(messagesPerField.getFirstError('username','password'))?no_esc}
                                </span>
                            </#if>
                        </div>

                        <div class="form-group">
                            <label for="password" class="form-label">${msg("password")}</label>
                            <div class="password-input-group">
                                <input tabindex="2" id="password" class="form-control" name="password" type="password" autocomplete="off"
                                       aria-invalid="<#if messagesPerField.existsError('username','password')>true</#if>"
                                />
                                <button type="button" class="toggle-password" onclick="togglePasswordVisibility()">
                                    <i class="fa fa-eye"></i>
                                </button>
                            </div>
                            <#if usernameEditDisabled?? && messagesPerField.existsError('username','password')>
                                <span id="input-error" class="form-error" aria-live="polite">
                                    ${kcSanitize(messagesPerField.getFirstError('username','password'))?no_esc}
                                </span>
                            </#if>
                        </div>

                        <div class="form-group form-check">
                            <#if realm.rememberMe && !usernameEditDisabled??>
                                <div class="checkbox">
                                    <label>
                                        <#if login.rememberMe??>
                                            <input tabindex="3" id="rememberMe" name="rememberMe" type="checkbox" checked> ${msg("rememberMe")}
                                        <#else>
                                            <input tabindex="3" id="rememberMe" name="rememberMe" type="checkbox"> ${msg("rememberMe")}
                                        </#if>
                                    </label>
                                </div>
                            </#if>
                        </div>

                        <div id="kc-form-buttons" class="form-group">
                            <input type="hidden" id="id-hidden-input" name="credentialId" <#if auth.selectedCredential?has_content>value="${auth.selectedCredential}"</#if>/>
                            <input tabindex="4" class="btn btn-primary btn-block" name="login" id="kc-login" type="submit" value="${msg("doLogIn")}"/>
                        </div>

                        <div class="form-group">
                            <div id="kc-form-options">
                                <#if realm.resetPasswordAllowed>
                                    <span><a tabindex="5" href="${url.loginResetCredentialsUrl}">${msg("doForgotPassword")}</a></span>
                                </#if>
                            </div>

                            <#if realm.password && realm.registrationAllowed && !registrationDisabled??>
                                <div id="kc-registration">
                                    <span>${msg("noAccount")} <a tabindex="6" href="${url.registrationUrl}">${msg("doRegister")}</a></span>
                                </div>
                            </#if>
                        </div>
                        
                        <div class="form-group">
                            <button type="button" id="toggle-dark-mode" class="btn btn-outline-secondary">
                                <i class="fa fa-moon-o"></i> Chế độ tối
                            </button>
                        </div>
                    </form>
                </#if>
            </div>
        </div>
        
        <script>
            function togglePasswordVisibility() {
                var passwordInput = document.getElementById('password');
                var icon = document.querySelector('.toggle-password i');
                
                if (passwordInput.type === 'password') {
                    passwordInput.type = 'text';
                    icon.classList.remove('fa-eye');
                    icon.classList.add('fa-eye-slash');
                } else {
                    passwordInput.type = 'password';
                    icon.classList.remove('fa-eye-slash');
                    icon.classList.add('fa-eye');
                }
            }
            
            document.getElementById('toggle-dark-mode').addEventListener('click', function() {
                document.body.classList.toggle('dark-mode');
                
                // Lưu trạng thái dark mode vào localStorage
                if(document.body.classList.contains('dark-mode')) {
                    localStorage.setItem('darkMode', 'enabled');
                    this.innerHTML = '<i class="fa fa-sun-o"></i> Chế độ sáng';
                } else {
                    localStorage.setItem('darkMode', 'disabled');
                    this.innerHTML = '<i class="fa fa-moon-o"></i> Chế độ tối';
                }
            });
            
            // Kiểm tra trạng thái dark mode khi tải trang
            if(localStorage.getItem('darkMode') === 'enabled') {
                document.body.classList.add('dark-mode');
                document.getElementById('toggle-dark-mode').innerHTML = '<i class="fa fa-sun-o"></i> Chế độ sáng';
            }
        </script>
    </#if>
</@layout.registrationLayout>
