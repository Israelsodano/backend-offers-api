![logo-quero-edu-small](https://user-images.githubusercontent.com/1139813/90247813-c9cfc780-de0d-11ea-9a97-485a7212d9dd.png)

## Observações sobre a solution:
- Os projetos "Common" foram pensados para ser um "base-code" em "artefatos nuget", versionados e reutilizaveis para outras aplicações.
- Foi utilizado "pipelines" de "log" e "transaction", desse jeito mantemos a integridade da base e registro de "requests" e "responses".
- O Repositorio criado em "Common" foi construido pensando em eventos e versionamento de entidade. 

## Instruções para executar a aplicação: 

A solução foi construída com base em docker-compose.

Sendo assim, executaremos nossa aplicação com apenas 2 passos.

- com o terminal abra a pasta raiz do projeto contendo nosso arquivo docker-compose.yml.
- execute o comando "docker-compose up -d"

### Simples assim !!!

- nossa aplicação ja deve estar rodando na url: <a href="https://localhost:5001/swagger/index.html">https://localhost:5001/swagger</a> 

