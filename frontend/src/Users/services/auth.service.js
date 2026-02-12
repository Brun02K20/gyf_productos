import axios from "axios";
import { BACKEND_URL } from "../../constants/constants";

// data: {username: "", password: ""}
const login = async (data) => {
    try {
        const login = await axios.post(`${BACKEND_URL}/api/users/login`, data);
        return login.data;
    } catch (error) {
        if (error.response) {
            return error.response.data
        }
    }
}

const register = async (data) => {
    try {
        const register = await axios.post(`${BACKEND_URL}/api/users`, data);
        return register.data;
    } catch (error) {
        if (error.response) {
            return error.response.data
        }
    }
}

const loginServices = {
    login,
    register
}

export { loginServices }