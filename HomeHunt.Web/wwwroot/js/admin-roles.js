const roleSelect = document.querySelector("#Input_NewRole");
const agentFields = document.querySelector("#agentFields");

if (roleSelect && agentFields) {
    const updateAgentFields = () => {
        agentFields.hidden = roleSelect.value !== agentFields.dataset.agentRole;
    };

    roleSelect.addEventListener("change", updateAgentFields);
    updateAgentFields();
}
