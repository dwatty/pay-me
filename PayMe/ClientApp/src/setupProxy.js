const createProxyMiddleware = require('http-proxy-middleware');
const { env } = require('process');

let target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:51841';

// I don't know
target = "http://localhost:5000";

module.exports = function(app) {

    const signalrProxy = createProxyMiddleware(['/hubs/gamehub'], {
        target: target,
        ws: true,
        changeOrigin: true,
        secure: false
    });

    const apiProxy = createProxyMiddleware(['/game'], {
        target: target,
        secure: false
    });

    app.use(signalrProxy);
    app.use(apiProxy);

};
