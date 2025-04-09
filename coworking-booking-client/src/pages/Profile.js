import { useContext, useEffect, useState } from "react";
import axios from "axios";
import { AuthContext } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";

const Profile = () => {
  const { token } = useContext(AuthContext);
  const navigate = useNavigate();
  const [user, setUser] = useState(null);
  const [payments, setPayments] = useState([]);

  useEffect(() => {
    if (!token) return navigate("/login");

    const fetchUser = async () => {
      try {
        const res = await axios.get("http://localhost:5228/api/users/me", {
          headers: { Authorization: `Bearer ${token}` },
        });
        setUser(res.data);
      } catch (err) {
        console.error("Ошибка получения профиля", err);
      }
    };

    const fetchPayments = async () => {
      try {
        const res = await axios.get("http://localhost:5228/api/users/me/payments", {
          headers: { Authorization: `Bearer ${token}` },
        });
        setPayments(res.data);
      } catch (err) {
        console.error("Ошибка загрузки оплат", err);
      }
    };

    fetchUser();
    fetchPayments();
  }, [token, navigate]);

  if (!user) return <p>Загрузка профиля...</p>;

  return (
    <div style={{ padding: "20px" }}>
      <h2>Профиль</h2>
      <p><strong>ID:</strong> {user.id}</p>
      <p><strong>Email:</strong> {user.email}</p>
      <p><strong>Имя пользователя:</strong> {user.name}</p>
      <p><strong>Дата регистрации:</strong> {new Date(user.registeredAt).toLocaleString()}</p>

      <h3 style={{ marginTop: "30px" }}>История оплат</h3>
      {payments.length === 0 ? (
        <p>Оплат пока нет</p>
      ) : (
        <table style={{ width: "100%", marginTop: "10px", borderCollapse: "collapse" }}>
          <thead>
            <tr>
              <th>Дата</th>
              <th>Сумма</th>
              <th>С</th>
              <th>По</th>
              <th>Статус</th>
            </tr>
          </thead>
          <tbody>
            {payments.map((p) => (
              <tr key={p.id}>
                <td>{new Date(p.paidAt).toLocaleString()}</td>
                <td>{p.amount} €</td>
                <td>{new Date(p.start).toLocaleTimeString()}</td>
                <td>{new Date(p.end).toLocaleTimeString()}</td>
                <td>{p.status}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default Profile;
