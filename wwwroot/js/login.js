window.logar = async function(email, password) {
    const response = await fetch("/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
        credentials: "include" // ESSENCIAL para o cookie ser armazenado
    });

    if (response.ok) {
        return true;
    } else {
        return false;
    }
};

window.logout = async function() {
    const response = await fetch("/logout", {
        method: "POST",
        headers: { "Content-Type": "application/json" }
    });

    if (response.ok) {
        return true;
    } else {
        return false;
    }
};
