import { createContext, useState } from "react";
import { useNavigate } from "react-router-dom";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const storedToken = localStorage.getItem("token");
  const [token, setToken] = useState(storedToken);
  const [userId, setUserId] = useState(null); // 
   
  

  const login = (newToken) => {
    localStorage.setItem("token", newToken);
    setToken(newToken);
  };

  
  const logout = () => {
  localStorage.removeItem("token");
  setToken(null);
  setUserId(null);
};

  return (
    <AuthContext.Provider value={{ token, login, logout, userId, setUserId }}>
      {children}
    </AuthContext.Provider>
  );
};