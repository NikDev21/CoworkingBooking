import { useContext, useEffect, useState } from "react";
import axios from "axios";
import { AuthContext } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";

const MyBookings = () => {
  const { token } = useContext(AuthContext);
  const navigate = useNavigate();
  const [bookings, setBookings] = useState([]);

  useEffect(() => {
    if (!token) return navigate("/login");

    const fetchBookings = async () => {
      try {
        const res = await axios.get("http://localhost:5228/api/bookings/mine", {
          headers: { Authorization: `Bearer ${token}` },
        });
        setBookings(res.data);
      } catch (err) {
        console.error("Ошибка загрузки бронирований", err);
      }
    };

    fetchBookings();
  }, [token, navigate]);

  const cancelBooking = async (id) => {
    if (!window.confirm("Вы уверены, что хотите отменить бронирование?")) return;

    try {
      await axios.delete(`http://localhost:5228/api/bookings/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });

      // удалим отменённую бронь из списка
      setBookings((prev) => prev.filter((b) => b.id !== id));
    } catch (err) {
      alert("Ошибка при отмене бронирования");
    }
  };

  return (
    <div style={{ padding: "30px" }}>
      <h2>Мои бронирования</h2>
      {bookings.length === 0 ? (
        <p>У вас пока нет бронирований.</p>
      ) : (
        <ul style={{ listStyle: "none", padding: 0 }}>
          {bookings.map((b) => (
            <li
              key={b.id}
              style={{
                marginBottom: "20px",
                padding: "16px",
                border: "1px solid #ccc",
                borderRadius: "8px",
              }}
            >
              <h4>
                {b.workspaceName} ({b.workspaceLocation})
              </h4>
              <p>
                <strong>С:</strong>{" "}
                {new Date(b.startDate).toLocaleString()} <br />
                <strong>По:</strong>{" "}
                {new Date(b.endDate).toLocaleString()}
              </p>
              <p>Стоимость: <strong>{b.totalPrice} €</strong></p>
              <p>Статус: {b.isPaid ? "✅ Оплачено" : "❌ Не оплачено"}</p>
              <button
                onClick={() => cancelBooking(b.id)}
                style={{
                  marginTop: "10px",
                  backgroundColor: "#dc3545",
                  color: "white",
                  border: "none",
                  borderRadius: "6px",
                  padding: "8px 12px",
                  cursor: "pointer",
                }}
              >
                Отменить бронирование
              </button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default MyBookings;
