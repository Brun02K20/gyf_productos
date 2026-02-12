import apiClient from "../../services/apiClient";

const getProducts = async () => {
    try {
        const response = await apiClient.get("/api/products");
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

const getProductById = async (id) => {
    try {
        const response = await apiClient.get(`/api/products/${id}`);
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

const getProductsByBudget = async (budget) => {
    try {
        const response = await apiClient.get(`/api/products/price/${budget}`);
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

const createProduct = async (data) => {
    try {
        const response = await apiClient.post("/api/products", data);
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

const updateProduct = async (id, data) => {
    try {
        const response = await apiClient.put(`/api/products/${id}`, data);
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

const deleteProduct = async (id) => {
    try {
        const response = await apiClient.delete(`/api/products/${id}`);
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

const productsServices = {
    getProducts,
    getProductById,
    getProductsByBudget,
    createProduct,
    updateProduct,
    deleteProduct
}

export { productsServices }