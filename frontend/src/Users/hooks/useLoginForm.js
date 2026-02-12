import { useNavigate } from "react-router-dom";
import { loginServices } from "../services/auth.service";
import { useAuth } from "../../context/auth/AuthContext";
import { useState } from "react";

const useLoginForm = (type) => {
    const { login } = useAuth();
    const [error, setError] = useState(false);
    const navigate = useNavigate();

    const onSubmit = async (data) => {
        try {
            let userOnlineData;

            if (type == "login") {
                userOnlineData = await loginServices.login(data);
            } else if (type == "register") {
                userOnlineData = await loginServices.register(data);
            }

            if (userOnlineData?.code == 401) {
                setError(true);
            } else {
                setError(false);
                if (type == "register") {
                    navigate("/");
                } else if (type == "login") {
                    login(userOnlineData);
                    navigate("/products");
                }
            }
        } catch (err) {
            setError(true);
        }
    };

    return {
        onSubmit,
        error,
        setError,
    };
};

export default useLoginForm;