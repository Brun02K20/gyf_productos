import apiClient from "../../services/apiClient";

const getCategories = async () => {
    try {
        const response = await apiClient.get("/api/categories");
        return response.data;
    } catch (error) {
        if (error.response) {
            return error.response.data;
        }
        return {
            networkError: true,
            message: "Oops, ha ocurrido un error inesperado"
        };
    }
};

const categoriesServices = {
    getCategories
}

export { categoriesServices }