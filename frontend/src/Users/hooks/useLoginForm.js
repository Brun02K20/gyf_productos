import { useNavigate } from "react-router-dom";
import { loginServices } from "../services/auth.service";
import { useAuth } from "../../context/auth/AuthContext";
import { useState } from "react";

const useLoginForm = (type) => {
    const { login } = useAuth();
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();

    const onSubmit = async (data) => {
        try {
            let userOnlineData;

            if (type == "login") {
                userOnlineData = await loginServices.login(data);
            } else if (type == "register") {
                userOnlineData = await loginServices.register(data);
            }

            if (userOnlineData?.networkError) {
                setErrorMessage(userOnlineData.message || "Oops, ha ocurrido un error inesperado");
                return;
            }

            // Manejar errores por código
            if (userOnlineData?.code) {
                if (userOnlineData.code === 401) {
                    const message =
                        type === "login"
                            ? "Error: El username o contraseña son incorrectos"
                            : "Error al registrar. Por favor, verifica tus datos e intenta nuevamente.";
                    setErrorMessage(message);
                } else if (userOnlineData.code === 409 && type === "register") {
                    setErrorMessage("Este usuario ya está registrado. Ya puedes iniciar sesión.");
                } else {
                    setErrorMessage(userOnlineData.message || "Oops, ha ocurrido un error inesperado");
                }
            } else {
                setErrorMessage("");
                if (type == "register") {
                    navigate("/");
                } else if (type == "login") {
                    login(userOnlineData);
                    navigate("/products");
                }
            }
        } catch (err) {
            setErrorMessage("Oops, ha ocurrido un error inesperado");
        }
    };

    return {
        onSubmit,
        errorMessage,
        setErrorMessage,
    };
};

export default useLoginForm;