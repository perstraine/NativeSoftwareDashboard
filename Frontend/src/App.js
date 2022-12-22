import { BrowserRouter, Routes, Route } from "react-router-dom";
import Login from "./pages/login/Login";
import Dashboard from './pages/dashboard/Dashboard';
import CustomerDashboard from "./pages/customerdashboard/CustomerDashboard";

function App() {
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Login />} />
          <Route path="/dashboard" element={<Dashboard/>}/>
          <Route path="/customerdashboard" element={<CustomerDashboard/>}/>
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
