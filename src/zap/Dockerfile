FROM owasp/zap2docker-stable
ENV WEBAPP "http://localhost:3000"
ENV WEB_BASE_URL "http://localhost:3000"
ENV API_KEY 12345
RUN mkdir /tmp/reports
VOLUME [ "/tmp/reports" ]

RUN wget https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh && \
    chmod +x wait-for-it.sh

CMD wait-for-it.sh -t 30 $WEB_BASE_URL -- \ 
    zap-cli start --start-options "-config api.key=$API_KEY" && \
    zap-cli --api-key $API_KEY quick-scan -o "-config api.key=$API_KEY" $WEBAPP; \
    zap-cli --api-key $API_KEY report -o /tmp/reports/zap.html -f html && \
    zap-cli --api-key $API_KEY shutdown 
