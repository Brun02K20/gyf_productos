# Build stage
FROM node:20-alpine AS build
WORKDIR /app

# Copiar package files
COPY package*.json ./

# Instalar dependencias
RUN npm install

# Copiar código fuente
COPY . .

# Build
RUN npm run build

# Runtime stage
FROM node:20-alpine
WORKDIR /app

# Instalar serve para servir archivos estáticos
RUN npm install -g serve

# Copiar archivos compilados del stage anterior
COPY --from=build /app/dist ./dist

# Exponer puerto
EXPOSE 3000

# Comando para servir la aplicación
CMD ["serve", "-s", "dist", "-l", "3000"]
