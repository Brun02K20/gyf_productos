import { useState } from 'react'
import { AuthProvider } from './context/auth/AuthContext'
import { ToastProvider } from './context/toast/ToastContext'
import ProtectedRoute from './context/auth/ProtectedRoute'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { Login } from './Users/pages/Login'
import { Register } from './Users/pages/Register'
import { ErrorNotFound } from './pages/ErrorNotFound'
import { Products } from './Products/pages/Products'

function App() {

  return (
    <>
      <AuthProvider>
        <ToastProvider>
          <BrowserRouter>
            <Routes>
              <Route path="/" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/products" element={<ProtectedRoute ><Products /></ProtectedRoute>} />
              <Route path="*" element={<ErrorNotFound />} />
            </Routes>
          </BrowserRouter>
        </ToastProvider>
      </AuthProvider>
    </>
  )
}

export default App
