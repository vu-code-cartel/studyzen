import axios from 'axios';

// Change the URL according to your machine
export const SERVER_URL = 'https://localhost:7041';

export const axiosClient = axios.create();
axiosClient.defaults.headers.post['Content-Type'] = 'application/json';
axiosClient.defaults.headers.put['Content-Type'] = 'application/json';
axiosClient.defaults.headers.patch['Content-Type'] = 'application/json';
