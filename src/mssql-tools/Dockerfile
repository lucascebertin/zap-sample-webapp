FROM mcr.microsoft.com/mssql-tools

RUN apt-get update && apt-get install wget
RUN wget https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh && \
    chmod +x wait-for-it.sh

ENV DB db:1433

CMD ./wait-for-it.sh -t 15 $DB -- \
    /opt/mssql-tools/bin/sqlcmd -S $DB_FOR_SQLCMD -U sa -P $SA_PASSWORD -Q "If(db_id(N'VulnDB') IS NULL) CREATE DATABASE VulnDb"
