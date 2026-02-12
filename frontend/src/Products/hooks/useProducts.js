import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { productsServices } from "../services/products.service";
import { categoriesServices } from "../../Categories/services/categories.service";
import { useToast } from "../../context/toast/ToastContext";

const useProducts = () => {
  const { showToast } = useToast();
  const [products, setProducts] = useState([]);
  const [categories, setCategories] = useState([]);
  const [searchError, setSearchError] = useState("");
  const [modalError, setModalError] = useState("");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [modalMode, setModalMode] = useState("create");
  const [selectedProduct, setSelectedProduct] = useState(null);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [productToDelete, setProductToDelete] = useState(null);

  const {
    register: registerSearch,
    handleSubmit: handleSubmitSearch,
    reset: resetSearch,
    formState: { errors: searchErrors }
  } = useForm();

  const {
    register: registerModal,
    handleSubmit: handleSubmitModal,
    reset: resetModal,
    setValue,
    formState: { errors: modalErrors }
  } = useForm();

  const formatDate = (value) => {
    if (!value) return "";
    const [datePart] = value.split(" ");
    const [year, month, day] = datePart.split("-");
    if (!year || !month || !day) return value;
    return `${day}/${month}/${year}`;
  };

  const fetchProducts = async () => {
    const response = await productsServices.getProducts();
    if (response?.networkError) {
      setSearchError(response.message || "Oops, ha ocurrido un error inesperado");
      return;
    }

    if (!Array.isArray(response)) {
      setSearchError("No se pudieron obtener los productos.");
      return;
    }

    setProducts(response);
  };

  const fetchCategories = async () => {
    const response = await categoriesServices.getCategories();
    if (response?.networkError) {
      setSearchError(response.message || "Oops, ha ocurrido un error inesperado");
      return;
    }

    if (!Array.isArray(response)) {
      setSearchError("No se pudieron obtener las categorias.");
      setCategories([]);
      return;
    }

    setCategories(response);
  };

  useEffect(() => {
    fetchProducts();
    fetchCategories();
  }, []);

  const handleSearch = async (data) => {
    setSearchError("");
    if (!data.budget) {
      const response = await productsServices.getProducts();
      if (response?.networkError) {
        setSearchError(response.message || "Oops, ha ocurrido un error inesperado");
        return;
      }

      if (!Array.isArray(response)) {
        setSearchError("No se pudieron obtener los productos.");
        setProducts([]);
        return;
      }
      setProducts(response);
      return;
    }

    const response = await productsServices.getProductsByBudget(Number(data.budget));
    if (response?.networkError) {
      setSearchError(response.message || "Oops, ha ocurrido un error inesperado");
      return;
    }

    if (!Array.isArray(response)) {
      setSearchError("No se pudieron obtener productos con ese precio.");
      setProducts([]);
      return;
    }

    setProducts(response);
  };

  const openCreateModal = () => {
    setModalMode("create");
    setSelectedProduct(null);
    setModalError("");
    resetModal();
    setIsModalOpen(true);
  };

  const openEditModal = (product) => {
    setModalMode("edit");
    setSelectedProduct(product);
    setModalError("");
    setValue("price", Number(product.price));
    
    // Buscar el categoryId por el categoryName
    const category = categories.find(cat => cat.name === product.categoryName);
    if (category) {
      setValue("categoryId", category.id);
    }
    
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setModalError("");
    resetModal();
    setSelectedProduct(null);
  };

  const handleCreate = async (data) => {
    const newProduct = await productsServices.createProduct({
      price: Number(data.price),
      categoryId: Number(data.categoryId)
    });
    if (newProduct?.networkError) {
      setModalError(newProduct.message || "Oops, ha ocurrido un error inesperado");
      return;
    }

    setModalError("");
    setProducts((prev) => [newProduct, ...prev]);
    showToast("Producto creado exitosamente", "success");
    closeModal();
  };

  const handleUpdate = async (data) => {
    if (!selectedProduct) return;

    const updatedProduct = await productsServices.updateProduct(selectedProduct.id, {
      price: Number(data.price),
      categoryId: Number(data.categoryId)
    });

    if (updatedProduct?.networkError) {
      setModalError(updatedProduct.message || "Oops, ha ocurrido un error inesperado");
      return;
    }

    setModalError("");
    setProducts((prev) =>
      prev.map((p) => (p.id === selectedProduct.id ? updatedProduct : p))
    );
    showToast("Producto actualizado exitosamente", "success");
    resetSearch();
    await fetchProducts();
    closeModal();
  };

  const handleModalSubmit = async (data) => {
    if (modalMode === "create") {
      await handleCreate(data);
    } else if (modalMode === "edit") {
      await handleUpdate(data);
    }
  };

  const openDeleteModal = (product) => {
    setProductToDelete(product);
    setIsDeleteModalOpen(true);
  };

  const closeDeleteModal = () => {
    setIsDeleteModalOpen(false);
    setProductToDelete(null);
  };

  const handleDelete = async () => {
    if (!productToDelete) return;

    const result = await productsServices.deleteProduct(productToDelete.id);

    if (result?.networkError) {
      showToast("Error al eliminar el producto", "error");
      closeDeleteModal();
      return;
    }

    setProducts((prev) => prev.filter((p) => p.id !== productToDelete.id));
    showToast("Producto eliminado exitosamente", "success");
    closeDeleteModal();
  };

  return {
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
  };
};

export default useProducts;
