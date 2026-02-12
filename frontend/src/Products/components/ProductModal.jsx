import React from "react";

const ProductModal = ({
  isOpen,
  onClose,
  mode,
  categories,
  register,
  handleSubmit,
  errors,
  onSubmit,
  error
}) => {
  if (!isOpen) return null;

  const titles = {
    create: "Crear producto",
    edit: "Editar producto"
  };

  const descriptions = {
    create: "Completa los datos del nuevo producto.",
    edit: "Modifica los datos del producto."
  };

  const buttonText = {
    create: "Guardar",
    edit: "Actualizar"
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 backdrop-blur-sm p-4">
      <div className="w-full max-w-md bg-linear-to-br from-white to-indigo-50/30 rounded-2xl shadow-2xl border border-indigo-200 p-6 space-y-5">
        <div className="flex items-start justify-between">
          <div>
            <h2 className="text-xl font-bold bg-linear-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent">{titles[mode]}</h2>
            <p className="text-sm text-indigo-600/70">{descriptions[mode]}</p>
          </div>
          <button
            type="button"
            onClick={onClose}
            className="text-indigo-500 hover:text-indigo-700 transition-colors cursor-pointer"
          >
            Cerrar
          </button>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <label htmlFor="price" className="block text-sm font-medium text-indigo-700">
              Precio
            </label>
            <input
              id="price"
              type="number"
              placeholder="Ej: 1500"
              {...register("price", {
                required: "El precio es requerido",
                valueAsNumber: true,
                min: { value: 1, message: "Debe ser mayor o igual a 1" },
                max: { value: 1000000, message: "Debe ser menor o igual a 1000000" },
                validate: (value) => Number.isInteger(value) || "Debe ser un numero entero"
              })}
              className={`w-full px-4 py-2.5 rounded-lg border transition-colors ${
                errors.price
                  ? "border-red-400 bg-red-50 focus:ring-2 focus:ring-red-200"
                  : "border-indigo-200 bg-white focus:border-transparent focus:ring-2 focus:ring-indigo-500"
              } outline-none text-gray-900 placeholder-indigo-400 shadow-sm`}
            />
            {errors.price && (
              <p className="text-xs font-medium text-red-600 mt-2">{errors.price.message}</p>
            )}
          </div>

          <div>
            <label htmlFor="categoryId" className="block text-sm font-medium text-indigo-700">
              Categoria
            </label>
            <select
              id="categoryId"
              {...register("categoryId", {
                required: "La categoria es requerida"
              })}
              className={`w-full px-4 py-2.5 rounded-lg border transition-colors ${
                errors.categoryId
                  ? "border-red-400 bg-red-50 focus:ring-2 focus:ring-red-200"
                  : "border-indigo-200 bg-white focus:border-transparent focus:ring-2 focus:ring-indigo-500"
              } outline-none text-gray-900 shadow-sm`}
            >
              <option value="">Selecciona una categoría</option>
              {categories.map((category) => (
                <option key={category.id} value={category.id}>
                  {category.name}
                </option>
              ))}
            </select>
            {errors.categoryId && (
              <p className="text-xs font-medium text-red-600 mt-2">{errors.categoryId.message}</p>
            )}

            {error && (
              <div className="mt-4 bg-red-50 border border-red-200 rounded-lg p-3 shadow-sm">
                <p className="text-sm font-medium text-red-700">{error}</p>
              </div>
            )}
          </div>

          <div className="flex gap-3">
            <button
              type="button"
              onClick={onClose}
              className="flex-1 px-4 py-2.5 rounded-lg border border-indigo-200 text-indigo-600 font-semibold hover:bg-indigo-50 transition-colors cursor-pointer"
            >
              Cancelar
            </button>
            <button
              type="submit"
              className="flex-1 px-4 py-2.5 rounded-lg bg-linear-to-r from-indigo-600 to-purple-600 text-white font-semibold shadow-lg shadow-indigo-500/50 hover:from-indigo-700 hover:to-purple-700 hover:shadow-xl hover:shadow-indigo-500/60 transition-all duration-300 cursor-pointer"
            >
              {buttonText[mode]}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ProductModal;
