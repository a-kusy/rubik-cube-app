import Sidebar from "../Sidebar/Sidebar"
import "./Login.css"
import { useState } from "react"
import axios from "axios"
import Cookies from 'js-cookie'

const Login = () => {
    const [data, setData] = useState({username: "", password: ""})
    const [dataReg, setDataReg] = useState({username: "", email: "", password: ""})
    const [error, setError] = useState("")
    const [errorReg, setErrorReg] = useState("")
    
    const handleLogin = async (e) =>{
        e.preventDefault()
        try{
            const url = "http://localhost:5124/Users/auth"
            const {data: res} = await axios.post(url, data)

            Cookies.set('token', res.token, {
                sameSite: 'lax', 
                secure: true
              });
            
            Cookies.set('username', res.username, {
                sameSite: 'lax', 
                secure: true
              });

            window.location = "/"
        } catch(error){
            setError(error.response.data.message)
        }
    }

    const handleRegister = async (e) =>{
        e.preventDefault()
        try{
            const url = "http://localhost:5124/Users/register"
            const {data: res} = await axios.post(url, dataReg)

            if(window.alert(res.message)){
                window.location.reload()
            }

        } catch(error){
            setErrorReg(error.response.data.message)
        }
    }

    const handleChange = ({ currentTarget: input }) => {
        setData({ ...data, [input.name]: input.value })
        
    };

    const handleChangeReg = ({ currentTarget: input }) => {
        setDataReg({ ...dataReg, [input.name]: input.value})
    };

    return (
        <div className="main-cont">
            <Sidebar></Sidebar>
            <div className="right-cont">
                <div className="cont-flex">
                    <div id="login">
                        <h3>LOGOWANIE</h3>
                        <form className="form-cont" onSubmit={handleLogin}>                         
                            <input
                                type="text"
                                name="username"
                                className="input"
                                placeholder="Login"
                                onChange={handleChange}
                                value={data.username}
                                maxLength="8"
                                required
                                title="Login może zawierać maksymalnie 8 znaków"
                            />
                            <input
                                type="password"
                                className="input"
                                placeholder="Hasło"
                                name="password"
                                onChange={handleChange}
                                value={data.password}
                                required
                                pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*_=+-]).{8,}$"
                                title="Hasło musi zawierać minimum 8 znaków w tym 1 dużą i małą literę, cyfrę oraz symbol"
                            /><br></br>
                            {error && <div>{error}</div>}
                            <button type="submit" className="button"> Zaloguj</button>
                        </form>

                    </div>
                    <div id="signup">
                        <h3>REJESTRACJA</h3>
                        <form className="form-cont" onSubmit={handleRegister}>
                            
                            <input
                                type="text"
                                name="username"
                                placeholder="Login"
                                className="input"
                                value={dataReg.username}
                                onChange={handleChangeReg}
                                required
                            />
                            
                            <input
                                type="password"
                                name="password"
                                placeholder="Hasło"
                                className="input"
                                value={dataReg.password}
                                onChange={handleChangeReg}
                                required
                                pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*_=+-]).{8,}$"
                                title="Hasło musi zawierać minimum 8 znaków w tym 1 dużą i małą literę, cyfrę oraz symbol"
                            />
                            
                            <input
                                type="email"
                                name="email"
                                placeholder="Email"
                                className="input"
                                value={dataReg.email}
                                onChange={handleChangeReg}
                                required
                            />
                            <br></br>
                            {errorReg && <div>{errorReg}</div>}
                            <button type="submit" className="button"> Załóż konto</button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    )
}
export default Login