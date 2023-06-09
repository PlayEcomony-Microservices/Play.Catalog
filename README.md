# PlayEconomy.Play.Catalog

A catalog microservice to facilitate catalog functionality in Play Economy Microservice Architecture. 
## Create and Publish Play.Catalog.Contracts package to GitHub

```powershell
$version="1.0.2"
$owner="PlayEcomony-Microservices"
$gh_pat="[PAT HERE]"
dotnet pack src\Play.Catalog.Contracts --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/Play.Catalog -o ..\packages
dotnet nuget push ..\packages\Play.Catalog.Contracts.$version.nupkg --api-key $gh_pat --source "github"
```

## Builder docker image

```powershell
$env:GH_OWNER="PlayEcomony-Microservices"
$env:GH_PAT="[PAT HERE]"
$acrName="playeconomybkm"
docker build --secret id=GH_OWNER --secret id=GH_PAT -t "$acrName.azurecr.io/play.catalog:$version" . 
```

## Run the docker image

```powershell
$cosmosDbConnStr="[CONN STRING HERE]"
$serviceBusConnString="[CONN STRING HERE]"
docker run -it --rm -p 5000:5000 --name catalog -e MongoDbSettings__ConnectionString=$cosmosDbConnStr -e ServiceBusSettings__ConnectionString=$serviceBusConnString -e ServiceSettings__MessageBroker="SERVICEBUS" play.catalog:$version
```

## Publish docker image to Azure Container Registry

```powershell
az acr login --name $acrName
docker push "$acrName.azurecr.io/play.catalog:$version"
```
