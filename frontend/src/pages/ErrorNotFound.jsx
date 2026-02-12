import React from 'react'

const ErrorNotFound = () => {
  return (
    <div className="min-h-screen bg-linear-to-br from-slate-50 to-slate-100 flex items-center justify-center p-4">
      <div className="w-full max-w-md">
        <div className="bg-white rounded-2xl shadow-xl p-8 space-y-6 text-center">
          <h1 className="text-3xl font-bold text-gray-900">404 - Página No Encontrada</h1>
          <p className="text-gray-500 text-sm mt-2">Lo sentimos, la página que buscas no existe.</p>

          <a href="/" className="inline-block mt-4 px-6 py-3 bg-blue-500 text-white rounded-lg hover:bg-blue-600 transition-colors">Volver al Inicio</a>
        </div>
       </div> 
       </div>
  )
}

export {ErrorNotFound}