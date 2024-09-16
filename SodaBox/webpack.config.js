const path = require('path');

module.exports = {
    entry: './wwwroot/js/nouislider-build.js',
    output: {
        filename: 'nouislider-bundle.js',  // Имя файла после сборки
        path: path.resolve(__dirname, 'wwwroot/js')
    },
    module: {
        rules: [
            {
                test: /\.css$/, // Обработка CSS
                use: ['style-loader', 'css-loader'],
            },
        ],
    },
    mode: 'development', // Режим разработки
};
