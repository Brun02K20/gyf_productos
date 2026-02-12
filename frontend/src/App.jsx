import { useState } from 'react'
import { AuthProvider } from './context/auth/AuthContext'
import ProtectedRoute from './context/auth/ProtectedRoute'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { Login } from './Users/pages/Login'
import { Register } from './Users/pages/Register'

function App() {

  return (
    <>
      <AuthProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/products" element={<ProtectedRoute ><p /></ProtectedRoute>} />
          </Routes>
        </BrowserRouter>
      </AuthProvider>
    </>
  )
}

export default App
