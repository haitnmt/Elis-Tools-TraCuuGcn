<#import "template.ftl" as layout>
<@layout.registrationLayout displayMessage=!messagesPerField.existsError('firstName','lastName','email','username','password','password-confirm'); section>
    <#if section = "header">
        ${msg("registerTitle")}
    <#elseif section = "form">
        <form id="kc-register-form" action="${url.registrationAction}" method="post">
            <div class="form-group">
                <label for="firstName" class="form-label">${msg("firstName")}</label>
                <input type="text" id="firstName" class="form-control" name="firstName"
                       value="${(register.formData.firstName!'')}"
                       aria-invalid="<#if messagesPerField.existsError('firstName')>true</#if>"
                />
                <#if messagesPerField.existsError('firstName')>
                    <span id="input-error-firstname" class="form-error" aria-live="polite">
                        ${kcSanitize(messagesPerField.get('firstName'))?no_esc}
                    </span>
                </#if>
            </div>

            <div class="form-group">
                <label for="lastName" class="form-label">${msg("lastName")}</label>
                <input type="text" id="lastName" class="form-control" name="lastName"
                       value="${(register.formData.lastName!'')}"
                       aria-invalid="<#if messagesPerField.existsError('lastName')>true</#if>"
                />
                <#if messagesPerField.existsError('lastName')>
                    <span id="input-error-lastname" class="form-error" aria-live="polite">
                        ${kcSanitize(messagesPerField.get('lastName'))?no_esc}
                    </span>
                </#if>
            </div>

            <div class="form-group">
                <label for="email" class="form-label">${msg("email")}</label>
                <input type="text" id="email" class="form-control" name="email"
                       value="${(register.formData.email!'')}" autocomplete="email"
                       aria-invalid="<#if messagesPerField.existsError('email')>true</#if>"
                />
                <#if messagesPerField.existsError('email')>
                    <span id="input-error-email" class="form-error" aria-live="polite">
                        ${kcSanitize(messagesPerField.get('email'))?no_esc}
                    </span>
                </#if>
            </div>

            <#if !realm.registrationEmailAsUsername>
                <div class="form-group">
                    <label for="username" class="form-label">${msg("username")}</label>
                    <input type="text" id="username" class="form-control" name="username"
                           value="${(register.formData.username!'')}" autocomplete="username"
                           aria-invalid="<#if messagesPerField.existsError('username')>true</#if>"
                    />
                    <#if messagesPerField.existsError('username')>
                        <span id="input-error-username" class="form-error" aria-live="polite">
                            ${kcSanitize(messagesPerField.get('username'))?no_esc}
                        </span>
                    </#if>
                </div>
            </#if>

            <div class="form-group">
                <label for="password" class="form-label">${msg("password")}</label>
                <div class="password-input-group">
                    <input type="password" id="password" class="form-control" name="password"
                           autocomplete="new-password"
                           aria-invalid="<#if messagesPerField.existsError('password','password-confirm')>true</#if>"
                    />
                    <button type="button" class="toggle-password" onclick="togglePasswordVisibility('password')">
                        <i class="fa fa-eye"></i>
                    </button>
                </div>
                <#if messagesPerField.existsError('password')>
                    <span id="input-error-password" class="form-error" aria-live="polite">
                        ${kcSanitize(messagesPerField.get('password'))?no_esc}
                    </span>
                </#if>
            </div>

            <div class="form-group">
                <label for="password-confirm" class="form-label">${msg("passwordConfirm")}</label>
                <div class="password-input-group">
                    <input type="password" id="password-confirm" class="form-control" name="password-confirm"
                           autocomplete="new-password"
                           aria-invalid="<#if messagesPerField.existsError('password-confirm')>true</#if>"
                    />
                    <button type="button" class="toggle-password" onclick="togglePasswordVisibility('password-confirm')">
                        <i class="fa fa-eye"></i>
                    </button>
                </div>
                <#if messagesPerField.existsError('password-confirm')>
                    <span id="input-error-password-confirm" class="form-error" aria-live="polite">
                        ${kcSanitize(messagesPerField.get('password-confirm'))?no_esc}
                    </span>
                </#if>
            </div>

            <#if recaptchaRequired??>
                <div class="form-group">
                    <div class="g-recaptcha" data-size="compact" data-sitekey="${recaptchaSiteKey}"></div>
                </div>
            </#if>

            <div class="form-group">
                <div id="kc-form-buttons" class="form-buttons">
                    <input class="btn btn-primary btn-block" type="submit" value="${msg("doRegister")}"/>
                    <a class="btn btn-default btn-block" href="${url.loginUrl}">${msg("doCancel")}</a>
                </div>
            </div>
            
            <div class="form-group">
                <button type="button" id="toggle-dark-mode" class="btn btn-outline-secondary">
                    <i class="fa fa-moon-o"></i> Chế độ tối
                </button>
            </div>
        </form>
        
        <script>
            function togglePasswordVisibility(inputId) {
                var passwordInput = document.getElementById(inputId);
                var icon = document.querySelector('#' + inputId).parentNode.querySelector('.toggle-password i');
                
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
