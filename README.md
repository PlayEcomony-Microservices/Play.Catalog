# PlayEconomy.Play.Catalog

A catalog microservice that hold catalog functionality. One of a few other microservices to make playEconomy a microservices architecture.

## Create and Publish Play.Catalog.Contracts package to GitHub

```powershell
$version=1.0.1
$owner="PlayEcomony-Microservices"
$gh_pat="[PAT HERE]"

dotnet pack src\Play.Catalog.Contracts --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/Play.Catalog -o ..\packages

dotnet nuget push ..\packages\Play.Catalog.Contracts.$version.nupkg --api-key $gh_pat --source "github"
```

## Builder docker image

```powershell
$env:GH_OWNER="PlayEcomony-Microservices"
$env:GH_PAT="[PAT HERE]"
docker build --secret id=GH_OWNER --secret id=GH_PAT -t play.catalog:$version . 
```

## Run the docker image

```powershell
docker run -it --rm -p 5000:5000 --name catalog -e MongoDbSettings__Host=mongo -e RabbitMQSettings__Host=rabbitmq  --network playinfra_default play.catalog:$version
```
