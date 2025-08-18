# Microsservice-Application ![CodeQL](https://github.com/RDPodcasting/Microsservice-Application/workflows/CodeQL/badge.svg)

Modelo para criação de Microsserviços ASP.NET Core no estado da arte 🚀

Para saber mais sobre templates, acesse [a documentação da microsoft](https://docs.microsoft.com/en-us/dotnet/core/tools/custom-templates).

## Instalação local com o projeto direto do github
Para realizar a instalação do Scaffold em .NET, acesse seu Prompt de Comando e digite o comando abaixo:

```
dotnet new install https://github.com/Thiago-Roock/Microsservice-Application/archive/refs/heads/main.zip --install-dir content/Microsservice

```

### Instalação local com projeto clonado na máquina
Para realizar a instalação do Scaffold em .NET, acesse seu Prompt de Comando e digite o comando abaixo:

Clone este projeto, vá para a pasta raiz do projeto e execute o seguinte comando:
```
dotnet new install ./content/Microsservice
```
#### Usando
Após a instalação, você pode executar `dotnet new list` para listar todos os templates instalados em sua máquina e verificar se ele contém este Scaffold.

Para criar um novo projeto com o modelo de trabalho, você pode executar o comando conforme o exemplo abaixo
```
dotnet new microsservice-api -n Sample -p ci-sample-api -o ci-sample-api
```
* Nome da solução: -n
* Nome do repositório Gitlab: -p
* Pasta de saída: -o
* Empty (um projeto sem qualquer amostra): -e

> Ref. https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-new
