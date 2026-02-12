import { useForm } from 'react-hook-form';
import useLoginForm from '../hooks/useLoginForm';

const Login = () => {    
     const {
        register,
        formState: { errors },
        handleSubmit,
    } = useForm();

    const { onSubmit, error, setError } = useLoginForm("login");

  return (
    <div className="min-h-screen bg-linear-to-br from-slate-50 to-slate-100 flex items-center justify-center p-4">
      <div className="w-full max-w-md">
        <div className="bg-white rounded-2xl shadow-xl p-8 space-y-6">
          {/* Header */}
          <div className="text-center">
            <h1 className="text-3xl font-bold text-gray-900">Bienvenido</h1>
            <p className="text-gray-500 text-sm mt-2">Inicia sesión en tu cuenta</p>
          </div>

          {/* Form */}
          <form id="login-htmlForm" onSubmit={handleSubmit(onSubmit)} className="space-y-5">
            {/* Username Input */}
            <div className="space-y-2">
              <label htmlFor="username" className="block text-sm font-medium text-gray-700">
                Nombre de Usuario
              </label>
              <input
                type="text"
                id="username"
                placeholder="tu_usuario"
                {...register("username", { required: true, onChange: () => setError(false) })}
                className={`w-full px-4 py-2.5 rounded-lg border transition-colors ${
                  errors.username
                    ? 'border-red-400 bg-red-50 focus:ring-2 focus:ring-red-200'
                    : 'border-gray-200 bg-gray-50 focus:border-transparent focus:ring-2 focus:ring-blue-500'
                } outline-none text-gray-900 placeholder-gray-400 shadow-sm`}
              />
              {errors.username && (
                <p className="text-xs font-medium text-red-600">
                  El username es requerido
                </p>
              )}
            </div>

            {/* Password Input */}
            <div className="space-y-2">
              <label htmlFor="password" className="block text-sm font-medium text-gray-700">
                Contraseña
              </label>
              <input
                type="password"
                id="password"
                placeholder="••••••••"
                onChange={() => setError(false)}
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
                  onChange: () => setError(false)
                })}
                className={`w-full px-4 py-2.5 rounded-lg border transition-colors ${
                  errors.password
                    ? 'border-red-400 bg-red-50 focus:ring-2 focus:ring-red-200'
                    : 'border-gray-200 bg-gray-50 focus:border-transparent focus:ring-2 focus:ring-blue-500'
                } outline-none text-gray-900 placeholder-gray-400 shadow-sm`}
              />
              {errors.password && (
                <p className="text-xs font-medium text-red-600">
                  {errors.password.message}
                </p>
              )}
            </div>

            {/* Error Message */}
            {error && (
              <div className="bg-red-50 border border-red-200 rounded-lg p-3 shadow-sm">
                <p className="text-sm font-medium text-red-700">
                  Error: El username o contraseña son incorrectos
                </p>
              </div>
            )}

            {/* Submit Button */}
            <button
              type="submit"
              className="w-full bg-linear-to-r from-blue-600 to-blue-700 hover:from-blue-700 hover:to-blue-800 text-white font-semibold py-2.5 rounded-lg transition-all duration-200 shadow-md hover:shadow-lg active:scale-95"
            >
              Iniciar sesión
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
          <p className="text-center text-sm text-gray-600">
            ¿No tienes cuenta?{" "}
            <a href="/register" className="font-semibold text-blue-600 hover:text-blue-700 transition-colors">
              Regístrate aquí
            </a>
          </p>
        </div>
      </div>
    </div>
  )
}

export {Login}
