# Análise de vulnerabilidades em aplicações web
O objetivo deste estudo é demonstrar, facilmente, como é possível encontrar problemas de segurança em aplicações web modernas utilizando ferramentas de automação de infraestrutura e de análise de vulnerabilidades.
Está fora do escopo, deste estudo, demonstrações de exploração das vulnerabilidades propositalmente adicionadas.
Como resultado, esperamos conseguir o máximo de informações que nos auxiliem a resolver rapidamente brechas de segurança.

## Definindo o perímetro
Parte fundamental da análise é delimitar o perímetro da busca por problemas.
Isto se explica por alguns motivos descritos abaixo:
- Feedback rápido
- Fácil identificação de responsáveis pela correção ou mitigação
- Previsibilidade dos efeitos colaterais
- Restrições em liberar informações sensíveis

Portanto, a análise será focada em uma API com alguns endpoints e ao estilo blackbox, vamos exercitar a visão de que não temos o código fonte e, mais importante, não precisamos do código para que seja possível encontrar falhas de segurança.

## Preparando o ambiente
Para automação, foi escolhido Docker como ferramenta base, assim torna-se rápido e fácil executar localmente tal qual um ambiente de nuvem qualquer, tendo em vista que todos permitem a execução de conteineres.
Instale a versão docker compatível com seu sistema operacional mas, após a instalação,  certifique-se de ter disponíveis os comandos `docker` e `docker-compose`. Sem eles, não poderemos prosseguir.

## Escolha das ferramentas de análise
Temos um órgão que encabeça uma iniciativa de avaliar e disponibilizar uma lista com as 10 falhas de segurança mais encontradas na internet.
A [OWASP](https://owasp.org) (Open Web Application Security Project), através do seu TOP 10, nos fornece um bom parâmetro para iniciarmos nossa análise. Não é necessário simplesmente aceitar a lista e ficar apenas com seus itens, sinta-se livre para inovar e adicionar outras ferramentas que façam análises mais aprofundadas de acordo com seu cenário e preocupação.
Por hora, vamos utilizar o [ZAP](https://www.owasp.org/index.php/OWASP_Zed_Attack_Proxy_Project) versão 2.8.0 (Zed Attack Proxy) dentro de nosso ambiente de conteineres.

## ZAP e linha de comando
Meu primeiro contato com o ZAP foi através de uma interface gráfica que já vem embutida nele.
É muito interessante para poder testar sem gastar muito tempo aprendendo mas, quando estamos falando de automação, queremos deixar o processo independente de interação humana, ou seja, a GUI fica completamente obsoleta.
Contudo, existe um projeto que estende o ZAP e entrega linha de comando para que possamos automatizar tudo, chama-se [zap-cli](https://github.com/Grunny/zap-cli), dentro dele a comunicação se dá via [API REST](https://github.com/zaproxy/zaproxy/wiki/ApiDetails#the-zap-api)
Antes de começar a se divertir com o ZAP via CLI, é preciso se atentar a um detalhe importante, a partir da versão 2.4.1 do ZAP (não do zap-cli), é necessário fornecer um valor alfanumérico como [chave da comunicação para API](https://github.com/zaproxy/zaproxy/wiki/FAQapikey).
Essa restrição foi adicionada por motivos de segurança, impedindo que aplicações maliciosas se utilizem do ZAP de forma arbitrária.

### Exemplo de como iniciar uma sessão, rodar um scan e, como output, gerar um relatório HTML:
```bash
export API_KEY=k33p4w4yh4ck3rz
export WEBAPP=http://minhapp.com/
zap-cli start --start-options "-config api.key=$API_KEY" # inicia o zap como daemon (headless)
zap-cli --api-key $API_KEY quick-scan -o "-config api.key=$API_KEY" $WEBAPP # inicia um scan completo na aplicação
zap-cli --api-key $API_KEY report -o /tmp/scan-report.html -f html # cria o report em uma pasta com permissão de escrita
zap-cli --api-key $API_KEY shutdown # finaliza o daemon do zap
```

## Executando a análise
Agora que sabemos um pouco mais sobre o ZAP, podemos rodar o sample e verificar os resultados gerados.
Para executar, rode em linha de comando:
```bash
docker-compose up
```

### Isto fará com que sejam criadas as imagens abaixo:
- SqlServer 2017 em Ubuntu.
- SqlServer tools (para interagir com o SqlServer e criar o database antes da aplicação iniciar).
- API com a falha de segurança (inicia migrations, cria tabelas e faz seed com alguns dados).
- ZAP com zap-cli pronto para fazer o scan.

### Após os downloads concluírem, a ordem de execução será:
1. SqlServer 2017 (banco disponível e aguardando conexões).
2. SqlServer tools (inicializa o database para a API).
3. Aplicação roda migrations e depois fica disponível para conexões.
4. Zap inicia, faz o scan, reporta os resultados na pasta `./volume/zap/reports/` e finaliza.

### Resutado:
Você deverá ver a seguinte informação na linha de comando:
```bash
[INFO]            Issues found: 1
+---------------+--------+----------+-------------------------------------------------------------------+
| Alert         | Risk   |   CWE ID | URL                                                               |
+===============+========+==========+===================================================================+
| SQL Injection | High   |       89 | http://webapp:3000/api/people?name=a%27+AND+%271%27%3D%271%27+--+ |
+---------------+--------+----------+-------------------------------------------------------------------+
[INFO]            Report saved to "/tmp/reports/zap.html"
```

O relatório html gerado pode ser visualizado [aqui](./volume/zap/reports/zap.html).

Comprovando que o ZAP conseguiu encontrar uma falha de `SQL Injection` em nossa aplicação de exemplo.

## Conclusão
1. Construímos uma infraestrutura para testar nossa aplicação.
2. Executamos de forma idempotente a aplicação e suas dependências.
3. Executamos uma análise de vulnerabilidades.
4. Extraímos, através do .html gerado, evidências da falha e algumas recomendações de correção.

Com isso temos informação suficiente para corrigir o problema e mitigar riscos futuros que podem comprometer nossa aplicação e, dependendo do grau de severidade, outros componentes ao redor da aplicação (servidores, redes, IoT, etc).

