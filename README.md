# 🚀 Minimal API com CancellationToken: Cancelamento Cooperativo

<p align="center">
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="SDK do .NET" />
  <img src="https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge&logo=dot-net&logoColor=white" alt="ASP.NET Core" />
  <img src="https://img.shields.io/badge/VS%20Code-007ACC?style=for-the-badge&logo=visual-studio-code&logoColor=white" alt="Visual Studio Code" />
</p>

Este projeto é um exemplo prático de **Minimal API** em **ASP.NET Core** que demonstra o conceito de **Cancelamento Cooperativo** usando o **`CancellationToken`**.

O objetivo é simples: garantir que o servidor pare de processar e libere recursos imediatamente se o cliente desconectar ou cancelar uma requisição de longa duração.

---

## 🌟 O Conceito Idiomático em Ação

Em **.NET**, o cancelamento de tarefas assíncronas deve ser **cooperativo** (a tarefa deve "checar" se foi cancelada), e o **`CancellationToken`** é a ferramenta padrão para isso.

Neste projeto, o ASP.NET Core:
* Associa o **`CancellationToken`** à propriedade `HttpContext.RequestAborted`.
* Sinaliza o token automaticamente se a conexão HTTP for encerrada pelo cliente (via *timeout* ou fechamento de aba/cliente).
* Os *endpoints* reagem a esse sinal, interrompendo o trabalho simulado (**`Task.Delay`**), lançando uma **`OperationCanceledException`** e finalizando a execução no servidor.

---

## 🛠️ Requisitos

Para rodar e testar esta API, você precisará:

* **SDK do .NET 8** (ou superior).
* **Visual Studio Code** (com a extensão **C# Dev Kit** é altamente recomendável).
* Um cliente HTTP para testes (Postman, Insomnia, ou o próprio arquivo `.http` do VS Code).

---

## 🏃 Como Executar

Siga estes passos no terminal integrado do VS Code, na pasta raiz do projeto:

1.  **Restaurar Pacotes (se necessário):**
    ```bash
    dotnet restore
    ```
2.  **Executar o Projeto:**
    ```bash
    dotnet run
    ```
O terminal mostrará a URL de escuta (ex: `http://localhost:5000/`).

---

## ⚙️ Endpoints para Teste

A API expõe dois *endpoints* para demonstrar o contraste:

| Método | Rota | Descrição | Comportamento em Caso de Cancelamento do Cliente |
| :--- | :--- | :--- | :--- |
| **GET** | `/with-cancelation-token` | **COOPERATIVO**: Honra o `CancellationToken`. | Interrompe imediatamente, retorna `499 Client Closed Request`. |
| **GET** | `/without-cancelation-token` | **NÃO COOPERATIVO**: Ignora o `CancellationToken`. | Continua o processamento no servidor até o fim, desperdiçando recursos. |

### Testando o Cancelamento

Para ver o **`CancellationToken`** em ação no *endpoint* cooperativo:

1.  Inicie uma requisição **GET** para `/with-cancelation-token` (a operação leva **5 segundos**).
2.  Antes que a requisição termine (durante esses 5 segundos), **cancele a requisição** no seu cliente HTTP (clicando em "Stop" ou fechando a aba/navegador).

**Resultado Esperado:** O log do servidor deve mostrar a mensagem **"Endpoint COOPERATIVO: CANCELADO"**, e o cliente deve receber um **status 499** (ou 0, dependendo do cliente).

---

## 🔎 Debugando no VS Code

Para entender o fluxo de cancelamento em detalhes:

1.  Abra o arquivo **`ProcessamentoService.cs`** (onde está a lógica).
2.  Defina um **breakpoint** dentro do bloco `try/catch` do método **`ProcessarCooperativoAsync`**.
3.  Inicie o **debug** (`F5` ou vá para a aba **Executar e Depurar**).
4.  Execute a requisição no cliente HTTP.
5.  Quando o **breakpoint** for atingido, **cancele a requisição no cliente**. Você verá o *debugger* pular para o bloco `catch (OperationCanceledException)`, provando que o cancelamento funcionou.
