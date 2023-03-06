import Sidebar from "../Sidebar/Sidebar";
import { useEffect, useState } from "react";
import axios from "axios";
import Cookies from 'js-cookie';

const Settings = () => {
  const token = Cookies.get("token");
  const [userData, setUserData] = useState([]);

  const [passwordChangeData, setPasswordChangeData] = useState({
    password: "",
    passwordConfirm: "",
  });
  const [loginChangeData, setLoginChangeData] = useState({
    login: ""
  })
  const [emailChangeData, setEmailChangeData] = useState({
    email: ""
  })
  const [message, setMessage] = useState([]);

  useEffect(() => {
    const GetUserData = async () => {
    if (token) {
      try {
        const config = {
          method: "get",
          url: "http://localhost:5124/Users/user",
          headers: {
            "Content-Type": "application/json", Authorization: "Bearer " + token,
          },
        };

        const { data: res } = await axios(config);
        setUserData(res);
      } catch (error) {
        if (error.response && error.response.status >= 400 && error.response.status <= 500) {
          console.text(error.response);
        }
      }
    }
  };

    GetUserData();
  }, [token, userData]);

  

  const ChangePassword = async (e) => {
    e.preventDefault();
    try {
      const config = {
        method: "put",
        url: "http://localhost:5124/Users/",
        headers: {
          "Content-Type": "application/json", Authorization: "Bearer " + token,
        },
        data: {
          password: passwordChangeData.password,
        }
      };

      if (passwordChangeData.password !== passwordChangeData.passwordConfirm) {
        setMessage("Hasła nie są jednakowe");
      } else {
        setMessage("");
        console.log("udana zmiana");
        setPasswordChangeData({ password: "", passwordConfirm: "" });

        await axios(config);

        if (window.confirm("Dane zostały poprawnie zmienione")) {
          window.location.reload();
        }
      }
    } catch (error) {
      if (error.response && error.response.status >= 400 && error.response.status <= 500) {
        console.log(error.response);
      }
    }
  };

  const ChangeEmail = async (e) => {
    e.preventDefault();
    try {
      const config = {
        method: "put",
        url: "http://localhost:5124/Users/",
        headers: {
          "Content-Type": "application/json", Authorization: "Bearer " + token,
        },
        data: {
          email: emailChangeData.email,
        }
      };

      setEmailChangeData({ email: "" });

      await axios(config);

      if (window.confirm("Dane zostały poprawnie zmienione")) {
        window.location.reload();
      }
    }
    catch (error) {
      if (error.response && error.response.status >= 400 && error.response.status <= 500) {
        console.log(error.response);
      }
    }
  }

  const ChangeLogin = async (e) => {
    e.preventDefault();
    try {
      const config = {
        method: "put",
        url: "http://localhost:5124/Users/",
        headers: {
          "Content-Type": "application/json", Authorization: "Bearer " + token,
        },
        data: {
          username: loginChangeData.login
        }
      };

      setLoginChangeData({ login: "" });

      const { data: res } = await axios(config);

      Cookies.set('username', res.username, {
        sameSite: 'lax', 
        secure: true
      });

      if (window.confirm("Dane zostały poprawnie zmienione")) {
        window.location.reload();
      }
    }
    catch (error) {
      if (error.response && error.response.status >= 400 && error.response.status <= 500) {
        window.alert(error.response.data.message);
      }
    }
  }

  function handlePasswordChange(e) {
    const value = e.target.value;
    setPasswordChangeData({
      ...passwordChangeData,
      [e.target.name]: value,
    });
  }

  function handleLoginChange(e) {
    const value = e.target.value;
    setLoginChangeData({
      ...loginChangeData,
      [e.target.name]: value,
    });
  }

  function handleEmailChange(e) {
    const value = e.target.value;
    setEmailChangeData({
      ...emailChangeData,
      [e.target.name]: value,
    });
  }

  const handleDeleteButton = async (e) => {
    e.preventDefault();
    try {
      const config = {
        method: "put",
        url: "http://localhost:5124/Users/",
        headers: {
          "Content-Type": "application/json", Authorization: "Bearer " + token,
        },
        data: {
          isArchival: true,
        }
      };

      if (window.confirm("Czy na pewno chcesz usunąć konto?")) {
        await axios(config);
        Cookies.remove('token')
        Cookies.remove('username')
        window.location = "/"
        window.alert("Konto zostało usunięte")
      }
    }
    catch (error) {
      if (error.response && error.response.status >= 400 && error.response.status <= 500) {
        console.log(error.response);
      }
    }
  }

  return (
    <div className="main-cont">
      <Sidebar></Sidebar>
      <div className="right-cont">
        <h1>Profil użytkownika</h1>
        <div>
          <h2>Dane konta</h2>

          Nazwa użytkownika: &nbsp;
          {userData.username}
          <br />
          Adres e-mail: &nbsp;
          {userData.email}

          <h2>Zmiana danych</h2>        
          <form onSubmit={ChangeLogin}>
            <input 
              type="text" 
              name="login" 
              id="username" 
              className="inputChange"
              placeholder="Nowa nazwa użytkownika"
              value={loginChangeData.login} 
              onChange={handleLoginChange}
              maxLength="8"
              required 
              title="Nazwa użytkownika może zawierać maksymalnie 8 znaków"/> &nbsp;
            <input type="submit" className="buttonChange" value="Zmień" />
          </form>
          <form onSubmit={ChangeEmail}>
            <input 
              type="email" 
              name="email" 
              id="email" 
              className="inputChange"
              placeholder="Nowy email"
              value={emailChangeData.email} 
              onChange={handleEmailChange} 
              required/> &nbsp;
            <input type="submit" className="buttonChange" value="Zmień" />
          </form>
          <h2>Zmiana hasła</h2>
          <form onSubmit={ChangePassword}>
            <input
              type="password"
              name="password"
              id="password"
              className="inputChange"
              placeholder="Nowe hasło"
              value={passwordChangeData.password}
              onChange={handlePasswordChange}
              pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*_=+-]).{8,}$"
              title="Hasło musi zawierać minimum 8 znaków w tym 1 dużą i małą literę, cyfrę oraz symbol"
              required
            />{" "}
            <br />
            <input
              type="password"
              name="passwordConfirm"
              id="passwordConfirm"
              className="inputChange"
              value={passwordChangeData.passwordConfirm}
              onChange={handlePasswordChange}
              placeholder="Powtórz hasło"
              pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*_=+-]).{8,}$"
              title="Hasło musi zawierać minimum 8 znaków w tym 1 dużą i małą literę, cyfrę oraz symbol"
              required
            />{" "}<br />
            <p>{message}</p>
            <input type="submit" className="buttonChange" value="Zmień" />
          </form>
          <button className="buttonChange" onClick={handleDeleteButton}>Usuń konto</button>
        </div>
      </div>
    </div>
  );
};

export default Settings;
