# PlayEconomy.Play.Catalog
A catalog microservice that hold catalog functionality. One of a few other microservices to make playEconomy a microservices architecture.

## Create and Publish Play.Catalog.Contracts package to Github
```powershell
$version=1.0.1
$owner="PlayEcomony-Microservices"
$gh_pat="[PAT HERE]"

dotnet pack src\Play.Catalog.Contracts --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/Play.Catalog -o ..\packages

dotnet nuget push ..\packages\Play.Catalog.Contracts.$version.nupkg --api-key $gh_pat --source "github"

```
