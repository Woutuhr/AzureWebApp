# Workshop Azure AD

## Azure AD Configuratie

1. Ga naar de [Azure Portal](https://portal.azure.com) en log in met je account.
2. Klik op **Azure Active Directory**.
3. Klik op **App registrations** -> **New registration**.
5. Geef de applicatie een naam, bijvoorbeeld **Workshop**.
6. Kies bij **Supported accounttypes** voor **Accounts in this organizational directory only (Default Directory only - Single tenant)**.
7. Klik op **Register**.

## Applicatie configureren

1. Maak een self signed certificate aan, dit kun je in Visual Studio doen door te klikken op **Tools** -> **Command Line** -> **Developer Command Prompt**.
2. Voer het volgende commando uit: ``` dotnet dev-certs https -ep ./certificate.crt --trust ```
3. Open de **Azure Portal** en ga naar **Azure Active Directory** -> **App registrations** -> **Jouw App Registratie**.
4. Klik op **Certificaten and secrets** -> **Upload certificate**.
5. Geef het certificaat een beschrijving/naam en klik op **Add**.
6. Kopieer de thumbprint van het net geuploade certificaat.
7. Ga in Visual Studio naar de appsettings.json en plak de thumbprint in het veld **CertificateThumbprint**.
8. Ga in de **Azure Portal** naar **Azure Active Directory** -> **App registrations** -> **Jouw App Registratie**.
9. Klik op **Overview** en kopieer de **Application (client) ID** en plak deze in het veld **ClientId** in de appsettings.json.
10. Klik op **Overview** en kopieer de **Directory (tenant) ID** en plak deze in het veld **TenantId** in de appsettings.json.

## Applicatie registreren in Azure AD
1. Ga in de **Azure Portal** naar **Azure Active Directory** -> **App registrations** -> **Jouw App Registratie**.
2. Klik onder **Manage** op **Authentication**.
3. In platform configuration klik op **Add a platform**.
4. Kies voor **Web**.
5. Voer bij **Redirect URIs** de volgende url in: ```https://localhost:7283/signin-oidc```.
6. Klik op **Configure**.

## Authenticatie toevoegen aan de applicatie
1. Open de **Program.cs**.
2. Voeg de volgende code toe op regel 10:
```csharp
IEnumerable<string>? initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ');

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd")
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
        .AddDownstreamWebApi("DownstreamApi", builder.Configuration.GetSection("DownstreamApi"))
        .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews().AddMvcOptions(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();
```

2. Voeg boven **app.useAuthorization();** de volgende regel toe:
```csharp
app.useAuthentication();
```

3. Klik met Rechtermuis op de **Views -> Shared** folder en klik op **Add** -> **View** -> **Razor View (Empty)**.
4. Noem deze **_LoginPartial.cshtml** en klik op add.
5. Voeg de volgende code toe in het aangemaakte bestand.
```cshtml
@using System.Security.Principal

<ul class="navbar-nav">
@if (User.Identity?.IsAuthenticated == true)
{
        <li class="nav-item">
            <span class="navbar-text text-dark">Hello @User.Identity?.Name!</span>
        </li>
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="MicrosoftIdentity" asp-controller="Account" asp-action="SignOut">Sign out</a>
        </li>
}
else
{
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="MicrosoftIdentity" asp-controller="Account" asp-action="SignIn">Sign in</a>
        </li>
}
</ul>
```
6. Open de **_Layout.cshtml** en voeg de volgende code toe op regel **29**.
```cshtml
<partial name="_LoginPartial" />
```

## Database

1. Open de **appsettings.json**.
2. Voeg een connection string to aan een lokale database die bij jou draait.
3. Open de **Tools -> Nuget Package Manager -> Package Manager Console**.
4. Voer het volgende commando uit: ```Update-Database```.

## Starten maar

1. Run de applicatie.
2. Kijk eventueel of de todo's werken.