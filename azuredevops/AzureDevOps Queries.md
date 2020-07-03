# Azure Devops Search query

## Get me the code files that aren't tests or controllers or js/ts/json/cshtml
`THINGTOSEARCHFOR NOT file:*Tests.cs NOT file:*Test.cs NOT file:*Controller.cs NOT file:*.js NOT file:*.ts NOT file:*.lock NOT file:*.cshtml NOT file:*.csproj NOT file:*.json`