🚀 Minimal API com CancellationToken (Exemplo Idiomático)
Este projeto é um exemplo de Minimal API em ASP.NET Core que demonstra o uso do Padrão Idiomático (CancellationToken) para lidar com requisições de longa duração. O objetivo é garantir que o servidor pare de processar e libere recursos imediatamente se o cliente desconectar ou cancelar a requisição.

🌟 O Conceito Idiomático em Ação
Em .NET, o cancelamento de tarefas assíncronas deve ser cooperativo, e o CancellationToken é a ferramenta padrão para isso.

Neste projeto, o ASP.NET Core:

Associa o CancellationToken à propriedade HttpContext.RequestAborted.

Sinaliza o token automaticamente se a conexão HTTP for encerrada pelo cliente.

O endpoint reage a esse sinal, interrompendo o Task.Delay (simulando um trabalho pesado) e lançando uma exceção para finalizar a execução no servidor.

🛠️ Requisitos
SDK do .NET 8 (ou superior).

Visual Studio Code (com a extensão C# Dev Kit para melhor experiência de debug).

Um cliente HTTP para testes (como Postman, Insomnia, ou o arquivo .http do VS Code).

🏃 Como Executar
Siga estes passos no terminal integrado do VS Code, na pasta raiz do projeto:

Restaurar Pacotes (se necessário):

Bash

dotnet restore
Executar o Projeto:

Bash

dotnet run
O terminal mostrará a URL de escuta (ex: http://localhost:5000/).

⚙️ Endpoints para Teste
A API expõe um único endpoint:

Método	Rota	Descrição
GET	/relatorio-lento	Simula uma operação que leva 5 segundos para ser concluída.

Exportar para as Planilhas
Testando o Cancelamento
Para ver o CancellationToken em ação:

Inicie uma requisição GET para http://localhost:<porta>/relatorio-lento.

Antes que a requisição termine (nos 5 segundos), cancele a requisição no seu cliente HTTP (ou feche o navegador).

Resultado Esperado: O servidor deve retornar um status 499 Client Closed Request e interromper o processamento imediatamente, economizando recursos.

🔎 Debugando no VS Code
Para entender o fluxo de cancelamento:

Abra o arquivo Program.cs.

Defina um breakpoint dentro do bloco try do endpoint /relatorio-lento.

Vá para a aba Executar e Depurar (Ctrl+Shift+D ou F5).

Execute a requisição no cliente HTTP.

Quando o breakpoint for atingido, cancele a requisição no cliente. Você verá o debugger pular para o bloco catch (OperationCanceledException).