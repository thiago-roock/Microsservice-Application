# Microsservice-Application ![CodeQL](https://github.com/RDPodcasting/BackgroundTasks-Application/workflows/CodeQL/badge.svg)

Modelo para criação de Works ASP.NET Core no estado da arte 🚀

Para saber mais sobre templates, acesse [a documentação da microsoft](https://docs.microsoft.com/en-us/dotnet/core/tools/custom-templates).

## Instalação
Para realizar a instalação do Scaffold em .NET, acesse seu Prompt de Comando e digite o comando abaixo:

Clone este projeto, vá para a pasta raiz do projeto e execute o seguinte comando:
```
dotnet new -i content/Work
```
### Usando
Após a instalação, você pode executar `dotnet new -l` para listar todos os templates instalados em sua máquina e verificar se ele contém este Scaffold.

Para criar um novo projeto com o modelo de trabalho, você pode executar o comando conforme o exemplo abaixo
```
dotnet new buildingblock -n Sample -p ci-work-sample -o ci-work-sample
```
* Nome da solução: -n
* Nome do repositório Gitlab: -p
* Pasta de saída: -o
* Empty (um projeto sem qualquer amostra): -e

> Ref. https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-new
