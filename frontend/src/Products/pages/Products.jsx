import React from "react";
import useProducts from "../hooks/useProducts";
import ProductModal from "../components/ProductModal";
import DeleteConfirmationModal from "../components/DeleteConfirmationModal";
import Toast from "../components/Toast";
import { useToast } from "../../context/toast/ToastContext";

const Products = () => {
  const { toast, closeToast } = useToast();
  const {
    products,
    categories,
    searchError,
    modalError,
    isModalOpen,
    modalMode,
    isDeleteModalOpen,
    productToDelete,
    registerSearch,
    handleSubmitSearch,
    searchErrors,
    registerModal,
    handleSubmitModal,
    modalErrors,
    handleSearch,
    handleModalSubmit,
    openCreateModal,
    openEditModal,
    closeModal,
    openDeleteModal,
    closeDeleteModal,
    handleDelete,
    formatDate
  } = useProducts();

return (
<div className="min-h-screen bg-linear-to-br from-blue-50 via-indigo-50 to-purple-50 p-4">
    <div className="max-w-6xl mx-auto space-y-6">
        <div className="bg-white/90 backdrop-blur-sm rounded-2xl shadow-xl border border-indigo-100 p-6">
            <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
                <div>
                    <h1 className="text-2xl font-bold bg-linear-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent">Productos</h1>
                    <p className="text-sm text-indigo-600/70">Busca productos por precio o crea nuevos productos.</p>
                </div>
                <button
                    type="button"
                    onClick={openCreateModal}
                    className="inline-flex items-center justify-center px-5 py-2.5 rounded-lg bg-linear-to-r from-indigo-600 to-purple-600 text-white font-semibold shadow-lg shadow-indigo-500/50 hover:from-indigo-700 hover:to-purple-700 hover:shadow-xl hover:shadow-indigo-500/60 transition-all duration-300 hover:scale-105 cursor-pointer"
                >
                    CREAR
                </button>
            </div>

            <form
                onSubmit={handleSubmitSearch(handleSearch)}
                className="mt-6 flex flex-col gap-3 md:flex-row md:items-end"
            >
                <div className="flex-1">
                    <label htmlFor="budget" className="block text-sm font-medium text-indigo-700">
                        Buscar por precio
                    </label>
                    <input
                        id="budget"
                        type="number"
                        placeholder="Ej: 2500"
                        {...registerSearch("budget", {
                            valueAsNumber: false,
                            validate: (value) => {
                                if (!value) return true;
                                const num = Number(value);
                                if (!Number.isInteger(num)) return "Debe ser un numero entero";
                                if (num < 1) return "Debe ser mayor o igual a 1";
                                if (num > 1000000) return "Debe ser menor o igual a 1000000";
                                return true;
                            }
                        })}
                        className={`w-full px-4 py-2.5 rounded-lg border transition-colors ${
                            searchErrors.budget
                                ? "border-red-400 bg-red-50 focus:ring-2 focus:ring-red-200"
                                : "border-indigo-200 bg-white focus:border-transparent focus:ring-2 focus:ring-indigo-500"
                        } outline-none text-gray-900 placeholder-indigo-400 shadow-sm`}
                    />
                    {searchErrors.budget && (
                        <p className="text-xs font-medium text-red-600 mt-2">{searchErrors.budget.message}</p>
                    )}
                </div>
                <button
                    type="submit"
                    className="px-5 py-2.5 rounded-lg bg-linear-to-r from-slate-700 to-slate-900 text-white font-semibold shadow-lg shadow-slate-500/50 hover:from-slate-800 hover:to-black hover:shadow-xl hover:shadow-slate-500/60 transition-all duration-300 hover:scale-105 cursor-pointer"
                >
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="size-6">
  <path fillRule="evenodd" d="M10.5 3.75a6.75 6.75 0 1 0 0 13.5 6.75 6.75 0 0 0 0-13.5ZM2.25 10.5a8.25 8.25 0 1 1 14.59 5.28l4.69 4.69a.75.75 0 1 1-1.06 1.06l-4.69-4.69A8.25 8.25 0 0 1 2.25 10.5Z" clipRule="evenodd" />
</svg>

                </button>
            </form>

            {searchError && (
                <div className="mt-4 bg-red-50 border border-red-200 rounded-lg p-3 shadow-sm">
                    <p className="text-sm font-medium text-red-700">{searchError}</p>
                </div>
            )}
        </div>

        <div className="bg-white/90 backdrop-blur-sm rounded-2xl shadow-xl border border-indigo-100 p-6">
            <div className="overflow-x-auto">
                <table className="min-w-full border-collapse">
                    <thead className="bg-linear-to-r from-indigo-50 to-purple-50">
                        <tr className="text-left text-xs uppercase tracking-wide text-indigo-700 font-semibold">
                            <th className="px-4 py-3">ID</th>
                            <th className="px-4 py-3">Precio</th>
                            <th className="px-4 py-3">Fecha</th>
                            <th className="px-4 py-3">Categoria</th>
                            <th className="px-4 py-3">Acciones</th>
                        </tr>
                    </thead>
                    <tbody className="bg-white">
                            {products.map((product, index) => (
                                    <tr key={product.id} className={`border-t border-indigo-100 hover:bg-indigo-50/50 transition-colors ${
                                        index % 2 === 0 ? 'bg-white' : 'bg-indigo-50/20'
                                    }`}>
                                            <td className="px-4 py-3 text-indigo-900 font-medium">{product.id}</td>
                                            <td className="px-4 py-3 text-emerald-600 font-semibold">{`$` + product.price}</td>
                                            <td className="px-4 py-3 text-slate-600">{formatDate(product.createdAt)}</td>
                                            <td className="px-4 py-3"><span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-purple-100 text-purple-800">{product.categoryName}</span></td>
                                            <td className="px-4 py-3">
                                                    <div className="flex gap-2">
                                                        <button
                                                                type="button"
                                                                onClick={() => openEditModal(product)}
                                                                className="px-3 py-1 rounded-lg bg-linear-to-r from-amber-500 to-orange-500 text-white text-sm font-medium shadow-md shadow-amber-500/50 hover:from-amber-600 hover:to-orange-600 hover:shadow-lg hover:shadow-amber-500/60 transition-all duration-300 hover:scale-105 cursor-pointer"
                                                        ><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="size-6">
  <path d="M21.731 2.269a2.625 2.625 0 0 0-3.712 0l-1.157 1.157 3.712 3.712 1.157-1.157a2.625 2.625 0 0 0 0-3.712ZM19.513 8.199l-3.712-3.712-8.4 8.4a5.25 5.25 0 0 0-1.32 2.214l-.8 2.685a.75.75 0 0 0 .933.933l2.685-.8a5.25 5.25 0 0 0 2.214-1.32l8.4-8.4Z" />
  <path d="M5.25 5.25a3 3 0 0 0-3 3v10.5a3 3 0 0 0 3 3h10.5a3 3 0 0 0 3-3V13.5a.75.75 0 0 0-1.5 0v5.25a1.5 1.5 0 0 1-1.5 1.5H5.25a1.5 1.5 0 0 1-1.5-1.5V8.25a1.5 1.5 0 0 1 1.5-1.5h5.25a.75.75 0 0 0 0-1.5H5.25Z" />
</svg>
</button>
                                                        <button
                                                                type="button"
                                                                onClick={() => openDeleteModal(product)}
                                                                className="px-3 py-1 rounded-lg bg-linear-to-r from-rose-500 to-red-600 text-white text-sm font-medium shadow-md shadow-rose-500/50 hover:from-rose-600 hover:to-red-700 hover:shadow-lg hover:shadow-rose-500/60 transition-all duration-300 hover:scale-105 cursor-pointer"
                                                        >

                                                            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="size-6">
  <path fillRule="evenodd" d="M16.5 4.478v.227a48.816 48.816 0 0 1 3.878.512.75.75 0 1 1-.256 1.478l-.209-.035-1.005 13.07a3 3 0 0 1-2.991 2.77H8.084a3 3 0 0 1-2.991-2.77L4.087 6.66l-.209.035a.75.75 0 0 1-.256-1.478A48.567 48.567 0 0 1 7.5 4.705v-.227c0-1.564 1.213-2.9 2.816-2.951a52.662 52.662 0 0 1 3.369 0c1.603.051 2.815 1.387 2.815 2.951Zm-6.136-1.452a51.196 51.196 0 0 1 3.273 0C14.39 3.05 15 3.684 15 4.478v.113a49.488 49.488 0 0 0-6 0v-.113c0-.794.609-1.428 1.364-1.452Zm-.355 5.945a.75.75 0 1 0-1.5.058l.347 9a.75.75 0 1 0 1.499-.058l-.346-9Zm5.48.058a.75.75 0 1 0-1.498-.058l-.347 9a.75.75 0 0 0 1.5.058l.345-9Z" clipRule="evenodd" />
</svg>

                                                        </button>
                                                    </div>
                                            </td>
                                    </tr>
                            ))}
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <ProductModal
        isOpen={isModalOpen}
        onClose={closeModal}
        mode={modalMode}
        categories={categories}
        register={registerModal}
        handleSubmit={handleSubmitModal}
        errors={modalErrors}
        onSubmit={handleModalSubmit}
        error={modalError}
    />

    <DeleteConfirmationModal
        isOpen={isDeleteModalOpen}
        onClose={closeDeleteModal}
        onConfirm={handleDelete}
        productName={productToDelete?.categoryName}
    />

    <Toast
        message={toast.message}
        isVisible={toast.visible}
        onClose={closeToast}
        type={toast.type}
    />
</div>
);
}

export {Products}