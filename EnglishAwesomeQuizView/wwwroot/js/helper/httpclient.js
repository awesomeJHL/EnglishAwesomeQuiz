//import axios from 'axios';

const httpclient = axios.create({
    //baseURL: 'http://localhost:5000/',
    baseURL: "https://awesome-quiz-example.azurewebsites.net/",
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
    }
});

httpclient.interceptors.response.use(
    function (response) {
        return response;
    }, 
    function (error) {
        let res = error.response;
        if (res.status == 401) {
            //window.location.href = 'https://example.com/login';
        }
        console.error('Looks like there was a problem. Status Code: ' + res.status);
        return Promise.reject(error);
    }
)