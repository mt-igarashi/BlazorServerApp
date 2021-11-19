export function submit(name)
{
    document.forms[name].submit();
}

export function click(id)
{
    document.querySelector(id).click();
}