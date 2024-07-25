/* eslint-disable react/prop-types */
import { useState, useEffect, createContext } from 'react';

// Создаем контекст
export const AuthContext = createContext();

// Создаем провайдер контекста
export function AuthProvider({ children }) {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [username, setUsername] = useState("");
  // Проверка куки при загрузке приложения
  useEffect(() => {
    const cookie = document.cookie.split('; ').find(row => row.startsWith('what-is-that'));
    if (cookie) {
      setIsAuthenticated(true);
    }
  }, []);

  // Объект контекста
  const authContextValue = {
    username,
    setUsername,
    isAuthenticated,
    setIsAuthenticated
  };

  return (
    <AuthContext.Provider value={authContextValue}>
      {children}
    </AuthContext.Provider>
  );
}