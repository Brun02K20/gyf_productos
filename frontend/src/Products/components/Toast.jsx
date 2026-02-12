import React, { useEffect } from "react";

const Toast = ({ message, isVisible, onClose, type = "success" }) => {
  useEffect(() => {
    if (isVisible) {
      const timer = setTimeout(() => {
        onClose();
      }, 3000);
      return () => clearTimeout(timer);
    }
  }, [isVisible, onClose]);

  if (!isVisible) return null;

  const bgColor = type === "success" ? "bg-gradient-to-r from-emerald-500 to-green-600" : "bg-gradient-to-r from-rose-500 to-red-600";

  return (
    <div className="fixed top-4 right-4 z-100 animate-slide-in">
      <div className={`${bgColor} text-white px-6 py-4 rounded-xl shadow-2xl border border-white/20 flex items-center gap-3 min-w-75`}>
        <svg
          className="w-5 h-5 shrink-0"
          fill="none"
          strokeLinecap="round"
          strokeLinejoin="round"
          strokeWidth="2"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          {type === "success" ? (
            <path d="M5 13l4 4L19 7" />
          ) : (
            <path d="M6 18L18 6M6 6l12 12" />
          )}
        </svg>
        <p className="font-medium">{message}</p>
        <button
          onClick={onClose}
          className="ml-auto text-white hover:text-gray-200 transition-colors cursor-pointer"
        >
          <svg className="w-4 h-4" fill="none" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" viewBox="0 0 24 24" stroke="currentColor">
            <path d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>
    </div>
  );
};

export default Toast;
