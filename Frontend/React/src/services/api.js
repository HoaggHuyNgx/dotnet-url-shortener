import axios from 'axios';
import { getSessionId } from '../utils/session';

const API_URL = 'http://localhost:5234/api';

const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

// Interceptor để đính kèm token hoặc session ID vào mỗi request
api.interceptors.request.use(
    (config) => {
        const user = JSON.parse(localStorage.getItem('user'));
        const sessionId = getSessionId(); // Lấy hoặc tạo session ID

        if (user && user.token) {
            config.headers.Authorization = `Bearer ${user.token}`;
        } else if (sessionId) {
            // Nếu không có user nhưng có session ID, gửi nó đi
            config.headers['X-Session-Id'] = sessionId;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

export default api;
