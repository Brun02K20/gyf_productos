import axios from "axios";
import { BACKEND_URL } from "../constants/constants";

const getStoredUser = () => {
    try {
        const stored = window.localStorage.getItem("GYFProductos");
        return stored ? JSON.parse(stored) : null;
    } catch (error) {
        return null;
    }
};

const getAuthToken = () => {
    const user = getStoredUser();
    return user?.token || user?.Token || null;
};

const apiClient = axios.create({
    baseURL: BACKEND_URL,
    headers: {
        "Content-Type": "application/json"
    }
});

apiClient.interceptors.request.use(
    (config) => {
        const token = getAuthToken();
        if (token) {
            config.headers = config.headers || {};
            if (!config.headers.Authorization) {
                config.headers.Authorization = `Bearer ${token}`;
            }
        }
        return config;
    },
    (error) => Promise.reject(error)
);

apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error?.response?.status === 401) {
            window.localStorage.removeItem("GYFProductos");
            if (window.location.pathname !== "/") {
                window.location.assign("/");
            }
        }
        return Promise.reject(error);
    }
);

export default apiClient;
