//import axios from 'axios';

const httpclient = axios.create({
    //baseURL: 'http://localhost:5000/',
    baseURL: "https://awesome-quiz-example.azurewebsites.net/",
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
    }
});