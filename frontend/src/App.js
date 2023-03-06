import { Route, Routes, Navigate } from "react-router-dom"
import Ranking from './components/Ranking/Ranking';
import Cube from './components/Cube/Cube';
import Theory from "./components/Theory/Theory";
import Statistics from "./components/Statistics/Statistics";
import Login from "./components/Login/Login";
import Settings from "./components/Settings/Settings";
import Cookies from 'js-cookie'
import jwtDecode from 'jwt-decode';

function isTokenExpired(token) {
  const expiresAt = jwtDecode(token).exp;
  console.log(expiresAt)
  return Date.now() > expiresAt * 1000;
}

function App() {
  var user = Cookies.get("username");
  var token = Cookies.get("token")

  if(token){
  if (isTokenExpired(token)) {
    Cookies.remove('token'); 
    Cookies.remove('username')
  }}

  return (
    <Routes>
      <Route path="/ranking" element={<Ranking />} />
      <Route path="/" element={<Cube />} />
      <Route path="/teoria" element={<Theory/>} />
      <Route path="/statystyki" element={<Statistics/>} />
      <Route path="/logowanie" element={<Login/>} />
      {user && <Route path="/profil" element={<Settings/>}/>}
      <Route path="/profil" element={<Navigate replace to="/logowanie"/>} />
    </Routes>
  );
}
export default App;
