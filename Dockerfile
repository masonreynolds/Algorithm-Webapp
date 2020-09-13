FROM nginx:alpine
EXPOSE 80
RUN mkdir -p /usr/share/nginx/html/Algorithm-Webapp
COPY build/wwwroot /usr/share/nginx/html/Algorithm-Webapp
