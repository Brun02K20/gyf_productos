import apiClient from "../../services/apiClient";

// data: {username: "", password: ""}
const login = async (data) => {
    try {
        const login = await apiClient.post("/api/users/login", data);
        return login.data;
    } catch (error) {
        if (error.response) {
            return error.response.data
        }
        return {
            networkError: true,
            message: "Oops, ha ocurrido un error inesperado"
        };
    }
}

const register = async (data) => {
    try {
        const register = await apiClient.post("/api/users", data);
        return register.data;
    } catch (error) {
        if (error.response) {
            return error.response.data
        }
        return {
            networkError: true,
            message: "Oops, ha ocurrido un error inesperado"
        };
    }
}

const loginServices = {
    login,
    register
}

export { loginServices }