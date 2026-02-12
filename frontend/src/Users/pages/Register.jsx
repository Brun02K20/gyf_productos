import { useForm } from 'react-hook-form';
import useLoginForm from '../hooks/useLoginForm';

const  Register = () => {
  const {
        register,
        formState: { errors },
        handleSubmit,
    } = useForm();

    const { onSubmit, errorMessage, setErrorMessage } = useLoginForm("register");

  return (
    <div className="min-h-screen bg-linear-to-br from-blue-50 via-indigo-50 to-purple-50 flex items-center justify-center p-4">
      <div className="w-full max-w-md">
        <div className="bg-white/90 backdrop-blur-sm rounded-2xl shadow-2xl border border-indigo-200 p-8 space-y-6">
          {/* Header */}
          <div className="text-center">
            <h1 className="text-3xl font-bold bg-linear-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent">Bienvenido</h1>
            <p className="text-indigo-600/70 text-sm mt-2">Crea tu cuenta</p>
          </div>

          {/* Form */}
          <form id="register-htmlForm" onSubmit={handleSubmit(onSubmit)} className="space-y-5">
            {/* Username Input */}
            <div className="space-y-2">
              <label htmlFor="username" className="block text-sm font-medium text-indigo-700">
                Nombre de Usuario
              </label>
              <input
                type="text"
                id="username"
                placeholder="tu_usuario"
                {...register("username", { required: true, onChange: () => setErrorMessage("") })}
                className={`w-full px-4 py-2.5 rounded-lg border transition-colors ${
                  errors.username
                    ? 'border-red-400 bg-red-50 focus:ring-2 focus:ring-red-200'
                    : 'border-indigo-200 bg-white focus:border-transparent focus:ring-2 focus:ring-indigo-500'
                } outline-none text-gray-900 placeholder-indigo-400 shadow-sm`}
              />
              {errors.username && (
                <p className="text-xs font-medium text-red-600">
                  El username es requerido
                </p>
              )}
            </div>

            {/* Password Input */}
            <div className="space-y-2">
              <label htmlFor="password" className="block text-sm font-medium text-indigo-700">
                Contraseña
              </label>
              <input
                type="password"
                id="password"
                placeholder="••••••••"
                onChange={() => setErrorMessage("")}
                {...register("password", {
                  required: {
                    value: true,
                    message: "El password es requerido",
                  },
                  minLength: {
                    value: 4,
                    message: "El password debe tener al menos 4 caracteres",
                  },
                  maxLength: {
                    value: 15,
                    message: "El password debe tener máximo 15 caracteres",
                  },
                  onChange: () => setErrorMessage("")
                })}
                className={`w-full px-4 py-2.5 rounded-lg border transition-colors ${
                  errors.password
                    ? 'border-red-400 bg-red-50 focus:ring-2 focus:ring-red-200'
                    : 'border-indigo-200 bg-white focus:border-transparent focus:ring-2 focus:ring-indigo-500'
                } outline-none text-gray-900 placeholder-indigo-400 shadow-sm`}
              />
              {errors.password && (
                <p className="text-xs font-medium text-red-600">
                  {errors.password.message}
                </p>
              )}
            </div>

            {/* Error Message */}
            {errorMessage && (
              <div className="bg-red-50 border border-red-200 rounded-lg p-3 shadow-sm">
                <p className="text-sm font-medium text-red-700">
                  {errorMessage}
                </p>
              </div>
            )}

            {/* Submit Button */}
            <button
              type="submit"
              className="w-full bg-linear-to-r from-indigo-600 to-purple-600 hover:from-indigo-700 hover:to-purple-700 text-white font-semibold py-2.5 rounded-lg transition-all duration-300 shadow-lg shadow-indigo-500/50 hover:shadow-xl hover:shadow-indigo-500/60 hover:scale-105 cursor-pointer"
            >
              Registrarse
            </button>
          </form>

          {/* Divider */}
          <div className="relative">
            <div className="absolute inset-0 flex items-center">
              <div className="w-full border-t border-gray-200"></div>
            </div>
            <div className="relative flex justify-center text-xs">
              <span className="px-2 bg-white text-gray-500">O</span>
            </div>
          </div>

          {/* Sign Up Link */}
          <p className="text-center text-sm text-indigo-600/70">
            ¿Ya tienes cuenta?{" "}
            <a href="/" className="font-semibold text-indigo-600 hover:text-purple-600 transition-colors">
              Inicia sesión aquí
            </a>
          </p>
        </div>
      </div>
    </div>
  )
}

export {Register}
