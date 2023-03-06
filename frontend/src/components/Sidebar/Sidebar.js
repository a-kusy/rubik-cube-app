import { ProSidebar, Menu, MenuItem, SidebarHeader, SidebarFooter, SidebarContent } from "react-pro-sidebar"
import React, { useState } from "react"
import { Link } from "react-router-dom"
import { FaBook, FaTrophy, FaUserCog } from "react-icons/fa"
import { FiLogOut, FiLogIn, FiArrowLeftCircle, FiArrowRightCircle } from "react-icons/fi"
import { BiLineChart } from "react-icons/bi"
import {IoCube} from "react-icons/io5"
import "./Sidebar.css"
import "react-pro-sidebar/dist/css/styles.css"
import Cookies from 'js-cookie'

const Sidebar = () => {
    const [menuCollapse, setMenuCollapse] = useState(false)
    const user = Cookies.get("username")

    const menuIconClick = () => {
        menuCollapse ? setMenuCollapse(false) : setMenuCollapse(true)
    };

    const ifLogged =() =>{
        if (user == null || user === "undefined") {
            return(
            <MenuItem icon={<FiLogIn />}>Logowanie<Link to="/logowanie"></Link></MenuItem>)
        } else {
            return (
            <MenuItem icon={<FiLogOut />} onClick={handleLogout}>Wylogowanie</MenuItem>)
        }
    }

    const handleLogout = () => {
        Cookies.remove('token')
        Cookies.remove('username')
        window.location = "/"
    }

    return (
        <div id="header">
            <ProSidebar collapsed={menuCollapse}>
                <SidebarHeader id="sidebarHeader">
                    <div className="logotext">
                    {menuCollapse ? (
                           <p>{user == null ? "G" : user.charAt(0)}</p>
                        ) : (
                            <p>{user == null ? "Gość" : user}</p>
                        )}
                    </div>
                    <div className="closemenu" onClick={menuIconClick}>
                        {menuCollapse ? (
                            <FiArrowRightCircle />
                        ) : (
                            <FiArrowLeftCircle />
                        )}
                    </div>
                </SidebarHeader>
                <SidebarContent>
                    <Menu iconShape="square">
                        <MenuItem icon={<IoCube />}>Układanie kostki<Link to="/"></Link></MenuItem>
                        <MenuItem icon={<FaBook />}>Teoria<Link to="/teoria"/></MenuItem>
                        <MenuItem icon={<FaTrophy />}> Ranking <Link to="/ranking" /> </MenuItem>
                        <MenuItem icon={<BiLineChart />}>Statystyki<Link to="/statystyki"/></MenuItem>
                        { user && <MenuItem icon={<FaUserCog />}>Profil użytkownika<Link to="/profil"></Link></MenuItem>}
                    </Menu>
                </SidebarContent>
                <SidebarFooter>
                    <Menu iconShape="square">
                        {ifLogged()}
                    </Menu>
                </SidebarFooter>
            </ProSidebar>
        </div>
    );
};
export default Sidebar;
