<#import "template.ftl" as layout>
<@layout.mainLayout active='account' bodyClass='user'; section>

    <div class="row">
        <div class="col-md-10">
            <h2>${msg("editAccountHtmlTitle")}</h2>
        </div>
        <div class="col-md-2 text-right">
            <button type="button" id="toggle-dark-mode" class="btn btn-outline-secondary">
                <i class="fa fa-moon-o"></i> Chế độ tối
            </button>
        </div>
    </div>

    <form action="${url.accountUrl}" class="form-horizontal" method="post">
        <input type="hidden" id="stateChecker" name="stateChecker" value="${stateChecker}">

        <#if !realm.registrationEmailAsUsername>
            <div class="form-group ${messagesPerField.printIfExists('username','has-error')}">
                <div class="form-group-header">
                    <label for="username" class="control-label">${msg("username")}</label>
                    <#if realm.editUsernameAllowed>
                        <span class="required">*</span>
                    </#if>
                </div>

                <div class="form-group-body">
                    <input type="text" class="form-control" id="username" name="username" <#if !realm.editUsernameAllowed>disabled="disabled"</#if> value="${(account.username!'')}"/>
                </div>
            </div>
        </#if>

        <div class="form-group ${messagesPerField.printIfExists('email','has-error')}">
            <div class="form-group-header">
                <label for="email" class="control-label">${msg("email")}</label>
                <span class="required">*</span>
            </div>

            <div class="form-group-body">
                <input type="text" class="form-control" id="email" name="email" value="${(account.email!'')}"/>
            </div>
        </div>

        <div class="form-group ${messagesPerField.printIfExists('firstName','has-error')}">
            <div class="form-group-header">
                <label for="firstName" class="control-label">${msg("firstName")}</label>
                <span class="required">*</span>
            </div>

            <div class="form-group-body">
                <input type="text" class="form-control" id="firstName" name="firstName" value="${(account.firstName!'')}"/>
            </div>
        </div>

        <div class="form-group ${messagesPerField.printIfExists('lastName','has-error')}">
            <div class="form-group-header">
                <label for="lastName" class="control-label">${msg("lastName")}</label>
                <span class="required">*</span>
            </div>

            <div class="form-group-body">
                <input type="text" class="form-control" id="lastName" name="lastName" value="${(account.lastName!'')}"/>
            </div>
        </div>

        <div class="form-group">
            <div class="form-group-body">
                <#if url.referrerURI??><a href="${url.referrerURI}">${kcSanitize(msg("backToApplication")?no_esc)}</a></#if>
                <button type="submit" class="btn btn-primary" name="submitAction" value="Save">${msg("doSave")}</button>
                <button type="submit" class="btn btn-default" name="submitAction" value="Cancel">${msg("doCancel")}</button>
            </div>
        </div>
    </form>
    
    <script>
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
</@layout.mainLayout>
